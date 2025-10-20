using Moq;
using System;
using TurmaMaisA.Models;
using TurmaMaisA.Models.Students;
using TurmaMaisA.Persistence.Interfaces;
using TurmaMaisA.Services.Students;
using TurmaMaisA.Services.Students.Dtos;
using TurmaMaisA.Utils.Exceptions;

namespace TurmaMaisA.Test.Services.Students
{
    public class StudentTests
    {

        private readonly IStudentService _service;
        private readonly Mock<IStudentRepository> _mockRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public StudentTests()
        {
            _mockRepository = new Mock<IStudentRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new StudentService(_mockRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact(DisplayName = "Create With Valid Input Should Return Correct Result")]
        public async Task Create_WhenValidInput_ShouldReturnCorrectResult()
        {
            //Arrange
            var organizationId = Guid.NewGuid();
            var newStudentDto = new StudentCreateDto()
            {
                Name = "João",
                Email = "joao@teste.com",
                Cpf = "487.818.080-38",
            };

            var submitedStudent = new Student()
            {
                Name = "João",
                Email = "joao@teste.com",
                Cpf = "487.818.080-38",
                RA = "1"
            };

            var returnedStudent = new StudentDto()
            {
                Id = Guid.NewGuid(),
                Name = "João",
                Email = "joao@teste.com",
                Cpf = "487.818.080-38",
                RA = "1"
            };

            _mockRepository.Setup(r => r.CreateAsync(submitedStudent)).Returns(Task.FromResult(returnedStudent));

            //Act
            var result = await _service.CreateAsync(newStudentDto, organizationId);

            //Assert
            _mockRepository.Verify(r => r.CreateAsync(
                It.Is<Student>(s =>
                s.Name == submitedStudent.Name &&
                s.Email == submitedStudent.Email &&
                s.Cpf == submitedStudent.Cpf &&
                s.RA == submitedStudent.RA
            )), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Create When Cpf Is Invalid Should Throw BusinessRoleException")]
        public async Task Create_WhenCpfIsInvalid_ShouldThrowBusinessRuleException()
        {
            //Arrange
            var organizationId = Guid.NewGuid();
            var newStudentDto = new StudentCreateDto()
            {
                Name = "João",
                Email = "joao@teste.com",
                Cpf = "000.000.000-00",
            };

            var expectedMessage = "CPF fornecido é inválido.";

            //Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessRuleException>(() => _service.CreateAsync(newStudentDto, organizationId));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact(DisplayName = "GetById When Student Exists Should Return StudentDto")]
        public async Task Get_WithExistentId_ShouldReturnStudentDto()
        {
            //Arrange
            var studentId = Guid.NewGuid();
            var studentDb = new Student()
            {
                Id = studentId,
                Name = "João",
                Email = "joao@teste.com",
                Cpf = "037.870.100-25",
                RA = "1"
            };

            _mockRepository.Setup(r => r.GetByIdAsync(studentId)).ReturnsAsync(studentDb);

            //Act
            var result = await _service.GetByIdAsync(studentId);
            
            //Assert
            Assert.NotNull(result);
            Assert.IsType<StudentDto>(result);
            Assert.Equal(result.Id, studentId);
        }

        [Fact(DisplayName = "GetById When Student Not Exists Should Throw NotFoundException")]
        public async Task Get_WithNonExistentId_ShouldThrowNotFoundException()
        {
            //Arrange
            var nonExistentStudentId = Guid.NewGuid();

            _mockRepository.Setup(r => r.GetByIdAsync(nonExistentStudentId)).Returns(Task.FromResult((Student?)null));
            var expectedMessage = $"The entity 'Student' with key '{nonExistentStudentId}' was not found.";

            //Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(nonExistentStudentId));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact(DisplayName = "GetAll When Students Exist Should Return StudentDto List")]
        public async Task GetAllAsync_WhenStudentsExist_ShouldReturnStudentDtoList()
        {
            // Arrange
            var studentsFromRepo = new List<Student>
            {
                new Student { Id = Guid.NewGuid(), Name = "João", Cpf = "",  RA = "1", Email = "joao@teste.com"},
                new Student { Id = Guid.NewGuid(), Name = "Maria", Cpf = "", RA = "2", Email = "maria@teste.com"}
            };

            _mockRepository.Setup(r => r.GetAllAsync(null)).ReturnsAsync(studentsFromRepo);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact(DisplayName = "GetAll When No Students Exist Should Return Empty List")]
        public async Task GetAllAsync_WhenNoStudentsExist_ShouldReturnEmptyList()
        {
            // Arrange
            var emptyListFromRepo = new List<Student>();

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
            var studentId = Guid.NewGuid();
            var dbStudent = new Student()
            {
                Id = studentId,
                Name = "João",
                Email = "joao@teste.com",
                Cpf = "487.818.080-38",
                RA = "1"
            };

            var studentUpdatedDto = new StudentUpdateDto()
            {
                Id = studentId,
                Name = "João",
                Email = "joao@teste.com"
            };

            _mockRepository.Setup(r => r.GetByIdAsync(studentId)).ReturnsAsync(dbStudent);

            //Act
            await _service.UpdateAsync(studentUpdatedDto);

            //Assert
            _mockRepository.Verify(r => r.Update(
                It.Is<Student>(s =>
                s.Id == dbStudent.Id &&
                s.Name == studentUpdatedDto.Name &&
                s.Email == studentUpdatedDto.Email &&
                s.Cpf == dbStudent.Cpf &&
                s.RA == dbStudent.RA
            )), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Update When Student Not Found Should Throw NotFoundException")]
        public async Task Update_WhenNotFoundId_ShouldThrowNotFoundException()
        {
            //Arrange
            var nonExistentStudentId = Guid.NewGuid();

            var studentUpdatedDto = new StudentUpdateDto()
            {
                Id = nonExistentStudentId,
                Name = "João",
                Email = "joao@teste.com"
            };

            var expectedMessage = $"The entity 'Student' with key '{nonExistentStudentId}' was not found.";

            //Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync(studentUpdatedDto));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact(DisplayName = "Delete When Student Is Found Should Return Correct Result")]
        public async Task Delete_WhenStudentIsFound_ShouldReturnCorrectResult()
        {
            //Arrange
            var studentId = Guid.NewGuid();
            var studentDb = new Student()
            {
                Id = studentId,
                Name = "João",
                Email = "joao@teste.com",
                Cpf = "037.870.100-25",
                RA = "1"
            };

            _mockRepository.Setup(r => r.GetByIdAsync(studentId)).ReturnsAsync(studentDb);

            //Act
            await _service.DeleteAsync(studentId);

            //Assert
            _mockRepository.Verify(r => r.Delete(studentDb), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Delete When Student Is Not Found Should Throw NotFoundException")]
        public async Task Delete_WhenStudentIsNotFound_ShouldThrowNotFoundException()
        {
            //Arrange
            var nonExistentStudentId = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetByIdAsync(nonExistentStudentId)).ReturnsAsync((Student?)null);

            var expectedMessage = $"The entity 'Student' with key '{nonExistentStudentId}' was not found.";

            //Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(nonExistentStudentId));
            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}
