using TurmaMaisA.Services.Auth.Dtos;
using TurmaMaisA.Services.Auth.DTOs;

namespace TurmaMaisA.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthDto> LoginAsync(LoginDTO dto);
    }
}
