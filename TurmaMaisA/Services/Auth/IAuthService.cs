using TurmaMaisA.Services.Auth.Dtos;

namespace TurmaMaisA.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResultDto> LoginAsync(LoginDTO dto);
        Task<AuthResultDto> RegisterUserAsync(RegisterDto dto);
    }
}
