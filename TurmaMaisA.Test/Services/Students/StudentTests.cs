using Moq;
using System;
using TurmaMaisA.Models;
using TurmaMaisA.Persistence.Interfaces;
using TurmaMaisA.Persistence.Repositories.Students;
using TurmaMaisA.Services.Students;
using TurmaMaisA.Services.Students.Dtos;
using TurmaMaisA.Utils.Exceptions;

namespace TurmaMaisA.Test.Services.Students
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

        [Fact(DisplayName = "Create_WithValidInput")]
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

        [Fact(DisplayName = "Create When Cpf Is Invalid Should Throw BusinessRoleException")]
        public async Task Create_WhenCpfIsInvalid_ShouldThrowBusinessRuleException()
        {
            //Arrange
            var newStudentDto = new StudentCreateDto()
            {
                Name = "João",
                Email = "joao@teste.com",
                Cpf = "000.000.000-00",
            };

            var expectedMessage = "Given CPF is invalid.";

            //Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessRuleException>(() => _service.CreateAsync(newStudentDto));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact(DisplayName = "Get By Id When Student Exists Should Return StudentDto")]
        public async Task Get_WithExistentId_ShouldReturnStudentDto()
        {
            //Arrange
            var studentId = Guid.NewGuid();
            var returnedStudent = new Student()
            {
                Id = studentId,
                Name = "João",
                Email = "joao@teste.com",
                Cpf = "037.870.100-25",
                RA = "1"
            };

            _mockRepository.Setup(r => r.GetByIdAsync(studentId)).Returns(Task.FromResult(returnedStudent));

            //Act
            var result = await _service.GetByIdAsync(studentId);
            
            //Assert
            Assert.NotNull(result);
            Assert.IsType<StudentDto>(result);
            Assert.Equal(result.Id, studentId);
        }

        [Fact(DisplayName = "Get By Id When Student Not Exists Should Throw NotFoundException")]
        public async Task Get_WithNonExistentId_ShouldThrowNotFoundException()
        {
            //Arrange
            var nonExistentStudentId = Guid.NewGuid();
            Student? nullStudent = null;

            _mockRepository.Setup(r => r.GetByIdAsync(nonExistentStudentId)).Returns(Task.FromResult(nullStudent));
            var expectedMessage = $"The entity 'Student' with key '{nonExistentStudentId}' was not found.";

            //Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(nonExistentStudentId));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact(DisplayName = "Update_WithValidInput")]
        public async Task Update_WithValidInput_ShouldReturnCorrectResult()
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

            _mockRepository.Setup(r => r.GetByIdAsync(studentId)).Returns(Task.FromResult(dbStudent));

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
        }

        [Fact(DisplayName = "Update_WhenStudentNotFound")]
        public async Task Update_WithNotFoundId_ShouldThrowNotFoundException()
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
    }
}
