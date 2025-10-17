using TurmaMaisA.Services.Auth.Dtos;

namespace TurmaMaisA.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResultDto> LoginAsync(LoginDto dto);
        Task<AuthResultDto> RegisterUserAsync(RegisterDto dto);
    }
}
