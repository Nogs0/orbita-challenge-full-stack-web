using Moq;
using TurmaMaisA.Models;
using TurmaMaisA.Models.Courses;
using TurmaMaisA.Models.Enrollments;
using TurmaMaisA.Models.Students;
using TurmaMaisA.Persistence.Interfaces;
using TurmaMaisA.Services.Courses.Dtos;
using TurmaMaisA.Services.Enrollments;
using TurmaMaisA.Services.Enrollments.Dtos;
using TurmaMaisA.Utils.Exceptions;

namespace TurmaMaisA.Test.Services.Enrollments
{
    public class EnrolmentTests
    {
        private readonly IEnrollmentService _service;
        private readonly Mock<IEnrollmentRepository> _mockRepository;
        private readonly Mock<IStudentRepository> _mockStudentRepository;
        private readonly Mock<ICourseRepository> _mockCourseRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public EnrolmentTests()
        {
            _mockRepository = new Mock<IEnrollmentRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockStudentRepository = new Mock<IStudentRepository>();
            _mockCourseRepository = new Mock<ICourseRepository>();

            _service = new EnrollmentService(_mockRepository.Object, _mockUnitOfWork.Object, _mockStudentRepository.Object, _mockCourseRepository.Object);
        }

        [Fact(DisplayName = "GetByStudentId Should Return EnrollmentDto List")]
        public async Task GetByStudentId_ShouldReturnEnrollmentDtoList()
        {
            //Arrange
            var studentId = Guid.NewGuid();
            var enrollmentsByUser = new List<Enrollment>
            {
                new()
                {
                    StudentId = studentId,
                    CourseId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                },
                new()
                {
                    StudentId = studentId,
                    CourseId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                }
            };

            _mockRepository.Setup(cr => cr.GetAllByStudentId(studentId)).ReturnsAsync(enrollmentsByUser);

            //Act
            var result = await _service.GetByStudentIdAsync(studentId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact(DisplayName = "Set Enrollments When The CoursesIds Are Valid Should Return EnrollmentDto List")]
        public async Task SetEnrollments_WhenTheCoursesIdsArevalid_ShouldReturnSettedEnrollmentDtoList()
        {
            //Arrange
            Guid studentId = Guid.NewGuid();
            Guid aedId = Guid.NewGuid();
            Guid iccId = Guid.NewGuid();
            Guid aocId = Guid.NewGuid();

            var student = new Student()
            {
                Id = studentId,
                Name = "João",
                Cpf = "062.873.700-92",
                Email = "joao@teste.com",
                RA = "1",
                OrganizationId = Guid.NewGuid()
            };

            var coursesDb = new List<Course>
            {
                new()
                {
                    Id = aedId,
                    Name = "Algoritmos e Estruturas de Dados I"
                },
                new()
                {
                    Id = iccId,
                    Name = "Introdução a Ciência da Computação"
                },
                new()
                {
                    Id = aocId,
                    Name = "Arquitetura e Organização de Computadores"
                }
            };

            var dto = new SetStudentEnrollmentsDto()
            {
                StudentId = studentId,
                CoursesIds = new List<Guid>
                {
                    aedId, iccId, aocId
                }
            };

            _mockCourseRepository.Setup(cr => cr.GetByIdsAsync(dto.CoursesIds)).Returns(Task.FromResult(coursesDb));
            _mockStudentRepository.Setup(sr => sr.GetByIdAsync(studentId)).ReturnsAsync(student);

            //Act
            var result = await _service.SetEnrolllmentsAsync(dto);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.True(result.All(x => dto.CoursesIds.Contains(x.CourseId)));
        }

        [Fact(DisplayName = "Set Enrollments When The CoursesIds Are Invalid Should Throw NotFoundException")]
        public async Task SetEnrollments_WhenTheCoursesIdsArevalid_ShouldThrowNotFoundException()
        {
            //Arrange
            Guid studentId = Guid.NewGuid();
            Guid aedId = Guid.NewGuid();
            Guid iccId = Guid.NewGuid();
            Guid aocId = Guid.NewGuid();

            var student = new Student()
            {
                Id = studentId,
                Name = "João",
                Cpf = "062.873.700-92",
                Email = "joao@teste.com",
                RA = "1",
                OrganizationId = Guid.NewGuid()
            };

            var coursesDb = new List<Course>
            {
                new()
                {
                    Id = aedId,
                    Name = "Algoritmos e Estruturas de Dados I"
                }
            };

            var dto = new SetStudentEnrollmentsDto()
            {
                StudentId = studentId,
                CoursesIds = new List<Guid>
                {
                    aedId, iccId, aocId
                }
            };

            var notFoundCourses = new List<Guid>()
            {
                iccId, aocId
            };

            _mockCourseRepository.Setup(cr => cr.GetByIdsAsync(dto.CoursesIds)).Returns(Task.FromResult(coursesDb));
            _mockStudentRepository.Setup(sr => sr.GetByIdAsync(studentId)).ReturnsAsync(student);
            var expectedMessage = $"The courses with the following keys are not found: {string.Join(", ", notFoundCourses)}";

            //Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _service.SetEnrolllmentsAsync(dto));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact(DisplayName = "Set Enrollments When The CoursesIds Are Null Or Empty Should Throw BusinessRuleException")]
        public async Task SetEnrollments_WhenTheCoursesIdsArevalid_ShouldThrowBusinessRuleException()
        {
            //Arrange
            Guid studentId = Guid.NewGuid();

            var student = new Student()
            {
                Id = studentId,
                Name = "João",
                Cpf = "062.873.700-92",
                Email = "joao@teste.com",
                RA = "1",
                OrganizationId = Guid.NewGuid()
            };

            var dto = new SetStudentEnrollmentsDto()
            {
                StudentId = studentId,
                CoursesIds = new List<Guid> { }
            };

            _mockStudentRepository.Setup(sr => sr.GetByIdAsync(studentId)).ReturnsAsync(student);
            var expectedMessage = "A lista de cursos deve conter ao menos um elemento.";

            //Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessRuleException>(() => _service.SetEnrolllmentsAsync(dto));
            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}
