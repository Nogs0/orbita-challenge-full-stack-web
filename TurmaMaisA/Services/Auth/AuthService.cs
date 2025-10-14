using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TurmaMaisA.Models;
using TurmaMaisA.Services.Auth.Dtos;
using TurmaMaisA.Services.Auth.DTOs;

namespace TurmaMaisA.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthDto> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);
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
