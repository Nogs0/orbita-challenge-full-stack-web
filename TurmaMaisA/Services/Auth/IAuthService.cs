using TurmaMaisA.Services.Auth.Dtos;

namespace TurmaMaisA.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthDto> LoginAsync(LoginDTO dto);
        Task<AuthDto> RegisterUserAsync(RegisterDto dto);
    }
}
