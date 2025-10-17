using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TurmaMaisA.Models;
using TurmaMaisA.Services.Auth;
using TurmaMaisA.Services.Auth.Dtos;
using TurmaMaisA.Services.Students.Dtos;
using TurmaMaisA.Utils.Exceptions;
using TurmaMaisA.Utils.Settings;

namespace TurmaMaisA.Test.Services.Auth
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<ILogger<AuthService>> _loggerMock;
        private readonly Mock<IOptions<JwtSettings>> _jwtSettings;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            var storeMock = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(
                storeMock.Object, null, null, null, null, null, null, null, null);

            _loggerMock = new Mock<ILogger<AuthService>>();
            var jwtSettings = new JwtSettings
            {
                Key = "6d382ec4-664a-43d3-9cf8-5a89b39fc965",
                Issuer = "TesteIssuer",
                Audience = "TesteAudience"
            };

            _jwtSettings = new Mock<IOptions<JwtSettings>>();
            _jwtSettings.Setup(o => o.Value).Returns(jwtSettings);

            _service = new AuthService(_mockUserManager.Object, _jwtSettings.Object, _loggerMock.Object);
        }

        [Fact(DisplayName = "Login When Credentials Are Valid Should Return Success")]
        public async Task Login_WithValidCredentials_ShouldReturnSuccessWithToken()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "joao@teste.com", Password = "Password123!" };
            var user = new User { FullName = "João Guilherme", Email = loginDto.Username, UserName = loginDto.Username };

            _mockUserManager.Setup(um => um.FindByEmailAsync(loginDto.Username))
                            .ReturnsAsync(user);
            _mockUserManager.Setup(um => um.CheckPasswordAsync(user, loginDto.Password))
                            .ReturnsAsync(true);

            // Act
            var result = await _service.LoginAsync(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Token);
            Assert.NotEmpty(result.Token);
            Assert.Null(result.ErrorMessage);
        }

        [Theory(DisplayName = "Login When Credentials Are Invalid Should Return Failure")]
        [InlineData(true, false)] // Usuário existe, senha errada
        [InlineData(false, false)] // Usuário não existe
        public async Task Login_WithInvalidCredentials_ShouldReturnFailure(bool userExists, bool passwordIsCorrect)
        {
            // Arrange
            var loginDto = new LoginDto { Username = "joao@teste.com", Password = "wrong-password" };
            User? user = userExists ? new User { FullName = "João Guilherme", Email = loginDto.Username } : null;

            _mockUserManager.Setup(um => um.FindByEmailAsync(loginDto.Username))
                            .ReturnsAsync(user);

            if (userExists)
            {
                _mockUserManager.Setup(um => um.CheckPasswordAsync(user!, loginDto.Password))
                                .ReturnsAsync(passwordIsCorrect);
            }

            var expectedErrorMessage = "Incorrect username or password.";

            // Act
            var result = await _service.LoginAsync(loginDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedErrorMessage, result.ErrorMessage);
            Assert.Null(result.Token);
        }

        [Fact(DisplayName = "RegisterUserAsync When User Is New Should Return Success")]
        public async Task RegisterUserAsync_WithNewUser_ShouldCreateUserAndReturnSuccess()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Email = "joao@teste.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                OrganizationName = "UniEsquina",
                FullName = "João Guilherme Testes"
            };

            _mockUserManager.Setup(um => um.FindByEmailAsync(registerDto.Email))
                            .ReturnsAsync((User?)null);

            _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), registerDto.Password))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _service.RegisterUserAsync(registerDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Token);
            Assert.NotEmpty(result.Token);
            Assert.Null(result.ErrorMessage);

            _mockUserManager.Verify(um => um.CreateAsync(
                It.Is<User>(u =>
                u.Email == registerDto.Email &&
                u.Organization.Name == registerDto.OrganizationName),
                registerDto.Password), Times.Once);
        }

        [Fact(DisplayName = "RegisterUserAsync When Email Exists Should Throw BusinessRuleException")]
        public async Task RegisterUserAsync_WhenEmailAlreadyExists_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Email = "joao@teste.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                OrganizationName = "UniEsquina",
                FullName = "João Guilherme Testes"
            };

            var existingUser = new User
            {
                Email = registerDto.Email,
                FullName = registerDto.FullName
            };

            _mockUserManager.Setup(um => um.FindByEmailAsync(registerDto.Email))
                            .ReturnsAsync(existingUser);

            var expectedMessage = "Email already registered.";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessRuleException>(() => _service.RegisterUserAsync(registerDto));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact(DisplayName = "RegisterUserAsync When Creation Fails Should Return Failure")]
        public async Task RegisterUserAsync_WhenUserManagerFails_ShouldReturnFailureWithErrors()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Email = "joao@teste.com",
                Password = "weak",
                ConfirmPassword = "weak",
                OrganizationName = "UniEsquina",
                FullName = "João Guilherme Testes"
            };

            var identityError = new IdentityError { Description = "Passwords must be at least 8 characters long and include uppercase letters, numbers, and special characters." };

            _mockUserManager.Setup(um => um.FindByEmailAsync(registerDto.Email))
                            .ReturnsAsync((User?)null);

            _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), registerDto.Password))
                            .ReturnsAsync(IdentityResult.Failed(identityError));

            // Act
            var result = await _service.RegisterUserAsync(registerDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.ErrorMessage);
            Assert.Contains(result.ErrorMessage, identityError.Description);
            Assert.Null(result.Token);
        }
    }
}
