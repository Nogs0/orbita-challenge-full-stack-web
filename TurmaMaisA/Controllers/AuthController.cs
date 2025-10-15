using Microsoft.AspNetCore.Mvc;
using TurmaMaisA.Services.Auth;
using TurmaMaisA.Services.Auth.Dtos;

namespace TurmaMaisA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.LoginAsync(dto);

            if (result.IsSuccess)
                return Ok(new { token = result.Token, expiration = result.TokenExpiration });

            return Unauthorized(new { message = result.ErrorMessage });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.RegisterUserAsync(dto);

            if (result.IsSuccess)
                return Ok(new { token = result.Token, expiration = result.TokenExpiration });

            return BadRequest(new { errors = result.ErrorMessage });
        }
    }
}
