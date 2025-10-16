using Moq;
using TurmaMaisA.Models;
using TurmaMaisA.Persistence.Interfaces;
using TurmaMaisA.Persistence.Repositories.Students;
using TurmaMaisA.Services.Students;
using TurmaMaisA.Services.Students.Dtos;

namespace TurmaMaisA.Test.Services
{
    public class StudentTests
    {

        private readonly IStudentService _service;
        private readonly Mock<IStudentRepository> _mockRepository;

        public StudentTests()
        {
            _mockRepository = new Mock<IStudentRepository>();
            var uow = new Mock<IUnitOfWork>();
            _service = new StudentService(_mockRepository.Object, uow.Object);
        }

        [Fact]
        public async Task Create_WithValidInput_ShouldReturnCorrectResult()
        {
            //Arrange
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
            var result = await _service.CreateAsync(newStudentDto);

            //Assert
            _mockRepository.Verify(r => r.CreateAsync(
                It.Is<Student>(s => 
                s.Name == submitedStudent.Name && 
                s.Email == submitedStudent.Email &&
                s.Cpf == submitedStudent.Cpf &&
                s.RA == submitedStudent.RA
            )), Times.Once);

        }
    }
}
