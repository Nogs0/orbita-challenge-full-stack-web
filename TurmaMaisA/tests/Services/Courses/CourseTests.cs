using Moq;
using TurmaMaisA.Models;
using TurmaMaisA.Models.Courses;
using TurmaMaisA.Models.Enrollments;
using TurmaMaisA.Persistence.Interfaces;
using TurmaMaisA.Services.Courses;
using TurmaMaisA.Services.Courses.Dtos;
using TurmaMaisA.Utils.Exceptions;

namespace TurmaMaisA.Test.Services.Courses
{
    public class CourseTests
    {
        private readonly ICourseService _service;
        private readonly Mock<ICourseRepository> _mockRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IEnrollmentRepository> _mockEnrollmentRepository;

        public CourseTests()
        {
            _mockRepository = new Mock<ICourseRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockEnrollmentRepository = new Mock<IEnrollmentRepository>();
            _service = new CourseService(_mockRepository.Object, _mockUnitOfWork.Object, _mockEnrollmentRepository.Object);
        }

        [Fact(DisplayName = "Create With Valid Input Should Return Correct Result")]
        public async Task Create_WhenValidInput_ShouldReturnCorrectResult()
        {
            //Arrange
            var newCourseDto = new CourseCreateDto()
            {
                Name = "Algoritmo e Estrutura de Dados I"
            };

            var submittedCourse = new Course()
            {
                Name = "Algoritmo e Estrutura de Dados I"
            };

            var returnedCourse = new Course()
            {
                Id = Guid.NewGuid(),
                Name = "Algoritmo e Estrutura de Dados I"
            };

            _mockRepository.Setup(r => r.CreateAsync(submittedCourse)).Returns(Task.FromResult(returnedCourse));

            //Act
            var result = await _service.CreateAsync(newCourseDto);

            //Assert
            _mockRepository.Verify(r => r.CreateAsync(
                It.Is<Course>(s =>
                s.Name == submittedCourse.Name
            )), Times.Once);

            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "GetById When Course Exists Should Return CourseDto")]
        public async Task Get_WithExistentId_ShouldReturnCourseDto()
        {
            //Arrange
            var courseId = Guid.NewGuid();
            var returnedCourse = new Course()
            {
                Id = courseId,
                Name = "Algoritmo e Estrutura de Dados I"
            };

            _mockRepository.Setup(r => r.GetByIdAsync(courseId)).ReturnsAsync(returnedCourse);

            //Act
            var result = await _service.GetByIdAsync(courseId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<CourseDto>(result);
            Assert.Equal(result.Id, courseId);
        }

        [Fact(DisplayName = "GetById When Course Not Exists Should Throw NotFoundException")]
        public async Task Get_WithNonExistentId_ShouldThrowNotFoundException()
        {
            //Arrange
            var nonExistentCourseId = Guid.NewGuid();
            Course? nullCourse = null;

            _mockRepository.Setup(r => r.GetByIdAsync(nonExistentCourseId)).ReturnsAsync(nullCourse);
            var expectedMessage = $"The entity 'Course' with key '{nonExistentCourseId}' was not found.";

            //Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(nonExistentCourseId));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact(DisplayName = "GetAll When Courses Exists Should Return CourseDto List")]
        public async Task GetAllAsync_WhenCoursesExists_ShouldReturnCourseDtoList()
        {
            // Arrange
            var coursesDb = new List<Course>
            {
                new Course { Id = Guid.NewGuid(), Name = "Algoritmo e Estrutura de Dados I"},
                new Course { Id = Guid.NewGuid(), Name = "Cálculo I" }
            };

            _mockRepository.Setup(r => r.GetAllAsync(null)).ReturnsAsync(coursesDb);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact(DisplayName = "GetAll When No Courses Exists Should Return Empty List")]
        public async Task GetAllAsync_WhenNoCoursesExists_ShouldReturnEmptyList()
        {
            // Arrange
            var emptyListFromRepo = new List<Course>();

            _mockRepository.Setup(r => r.GetAllAsync(null)).ReturnsAsync(emptyListFromRepo);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact(DisplayName = "Update When Valid Input Should Return Correct Result")]
        public async Task Update_WhenValidInput_ShouldReturnCorrectResult()
        {
            //Arrange
            var courseId = Guid.NewGuid();
            var courseDb = new Course()
            {
                Id = courseId,
                Name = "Algoritmo e Estrutura de Dados I"
            };

            var courseUpdateDto = new CourseUpdateDto()
            {
                Id = courseId,
                Name = "Introdução a Ciência da Computação"
            };

            _mockRepository.Setup(r => r.GetByIdAsync(courseId)).ReturnsAsync(courseDb);

            //Act
            await _service.UpdateAsync(courseUpdateDto);

            //Assert
            _mockRepository.Verify(r => r.Update(
                It.Is<Course>(s =>
                s.Id == courseDb.Id &&
                s.Name == courseUpdateDto.Name
            )), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Update When Course Not Found Should Throw NotFoundException")]
        public async Task Update_WhenNotFoundId_ShouldThrowNotFoundException()
        {
            //Arrange
            var nonExistentCourseId = Guid.NewGuid();

            var courseUpdateDto = new CourseUpdateDto()
            {
                Id = nonExistentCourseId,
                Name = "Introdução a Ciência da Computação"
            };

            var expectedMessage = $"The entity 'Course' with key '{nonExistentCourseId}' was not found.";

            //Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync(courseUpdateDto));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact(DisplayName = "Delete When Course Is Not Used By Student Should Return Correct Result")]
        public async Task Delete_WhenCourseIsNotUsedByStudent_ShouldReturnCorrectResult()
        {
            //Arrange
            var courseId = Guid.NewGuid();
            var courseDb = new Course()
            {
                Id = courseId,
                Name = "AOC I"
            };

            _mockRepository.Setup(r => r.GetByIdAsync(courseId)).ReturnsAsync(courseDb);
            _mockEnrollmentRepository.Setup(er => er.GetAllByCourseId(courseId)).ReturnsAsync([]);

            //Act
            await _service.DeleteAsync(courseId);

            //Assert
            _mockRepository.Verify(r => r.Delete(courseDb), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Delete When Course Is Not Found Should Throw NotFoundException")]
        public async Task Delete_WhenCourseIsNotFound_ShouldThrowNotFoundException()
        {
            //Arrange
            var nonExistentCourseId = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetByIdAsync(nonExistentCourseId)).ReturnsAsync((Course?)null);

            var expectedMessage = $"The entity 'Course' with key '{nonExistentCourseId}' was not found.";

            //Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(nonExistentCourseId));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact(DisplayName = "Delete When Course Is Used By Student Should Throw BusinessRuleException")]
        public async Task Delete_WhenCourseIsUsedByStudent_ShouldThrowBusinessRuleException()
        {
            //Arrange
            var courseId = Guid.NewGuid();
            var courseDb = new Course()
            {
                Id = courseId,
                Name = "AOC I"
            };

            var enrollment = new Enrollment()
            {
                Id = Guid.NewGuid(),
                StudentId = Guid.NewGuid(),
                CourseId = courseId,
            };

            _mockRepository.Setup(r => r.GetByIdAsync(courseId)).ReturnsAsync(courseDb);
            _mockEnrollmentRepository.Setup(er => er.GetAllByCourseId(courseId)).ReturnsAsync([enrollment]);

            var expectedMessage = "Não é possível excluir um curso que algum aluno esteja matrículado.";

            //Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessRuleException>(() => _service.DeleteAsync(courseId));
            Assert.Equal(expectedMessage, exception.Message);

        }
    }
}
