using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TurmaMaisA.Models;
using TurmaMaisA.Services.Auth.Dtos;

namespace TurmaMaisA.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<User> userManager, 
            IConfiguration configuration,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<AuthDto> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return new AuthDto { IsSuccess = false, ErrorMessage = "Usuário ou senha inválidos." };
            }

            var tokenDetails = GenerateJwtToken(user);

            return new AuthDto
            {
                IsSuccess = true,
                Token = tokenDetails.tokenString,
                TokenExpiration = tokenDetails.expiration
            };
        }

        public async Task<AuthDto> RegisterUserAsync(RegisterDto dto)
        {
            var userExists = await _userManager.FindByEmailAsync(dto.Email);
            if (userExists != null)
                throw new Exception("Email já cadastrado.");

            var organization = new Organization()
            {
                Name = dto.OrganizationName,
            };

            var user = new User()
            {
                Email = dto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = dto.Email,
                FullName = dto.FullName,
                Organization = organization
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return new AuthDto
                {
                    IsSuccess = false,
                    ErrorMessage = string.Join(",", result.Errors.Select(e => e.Description).ToArray())
                };
            }
            
            _logger.LogInformation($"Usuário {user.Email} criado com sucesso. Gerando token de login.", user.Email);

            var tokenDetails = GenerateJwtToken(user);

            return new AuthDto
            {
                IsSuccess = true,
                Token = tokenDetails.tokenString,
                TokenExpiration = tokenDetails.expiration
            };
        }

        private (string tokenString, DateTime expiration) GenerateJwtToken(User user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("OrganizationId", user.OrganizationId.ToString())
            };

            // TO DO: Lógica para buscar e adicionar roles...

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
        }
    }
}
