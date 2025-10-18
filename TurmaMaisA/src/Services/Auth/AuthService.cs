using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TurmaMaisA.Models;
using TurmaMaisA.Persistence.Repositories.Organizations;
using TurmaMaisA.Services.Auth.Dtos;
using TurmaMaisA.Utils.Exceptions;
using TurmaMaisA.Utils.Settings;

namespace TurmaMaisA.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthService> _logger;
        private readonly IOrganizationRepository _organizationRepository;

        public AuthService(UserManager<User> userManager,
            IOptions<JwtSettings> _optionsJwtSettings,
            ILogger<AuthService> logger,
            IOrganizationRepository organizationRepository)
        {
            _userManager = userManager;
            _jwtSettings = _optionsJwtSettings.Value;
            _logger = logger;
            _organizationRepository = organizationRepository;
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Credenciais inválidas." };
            }

            var organization = await _organizationRepository.GetByIdAsync(user.OrganizationId) ??
                throw new NotFoundException("Organization", user.OrganizationId);

            var tokenDetails = GenerateJwtToken(user);

            return new AuthResultDto
            {
                IsSuccess = true,
                Token = tokenDetails.Token,
                TokenExpiration = tokenDetails.Expiration,
                UserFullName = user.FullName,
                OrganizationName = organization.Name
            };
        }

        public async Task<AuthResultDto> RegisterUserAsync(RegisterDto dto)
        {
            var userExists = await _userManager.FindByEmailAsync(dto.Email);
            if (userExists != null)
                throw new BusinessRuleException("Email já cadastrado.");

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
                return new AuthResultDto
                {
                    IsSuccess = false,
                    ErrorMessage = string.Join(",", result.Errors.Select(e => e.Description).ToArray())
                };
            }

            _logger.LogInformation($"User {user.Email} created with success. Generating login token.", user.Email);

            var tokenDetails = GenerateJwtToken(user);

            return new AuthResultDto
            {
                IsSuccess = true,
                Token = tokenDetails.Token,
                TokenExpiration = tokenDetails.Expiration,
                UserFullName = user.FullName,
                OrganizationName = organization.Name
            };
        }

        private TokenDetailsDto GenerateJwtToken(User user)
        {
            if (string.IsNullOrEmpty(user.UserName))
                throw new BusinessRuleException("O email é obrigatório.");

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("OrganizationId", user.OrganizationId.ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.Now.AddHours(6),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new TokenDetailsDto() {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo 
            };
        }
    }
}
