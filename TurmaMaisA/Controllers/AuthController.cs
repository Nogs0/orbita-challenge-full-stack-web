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

        /// <summary>
        /// Realiza o login do usuário e retorna um token JWT.
        /// </summary>
        /// <param name="dto">Objeto com as credenciais de email e senha.</param>
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

        /// <summary>
        /// Realiza o cadastro de um novo usuário e retorna um token para login automático.
        /// </summary>
        /// <param name="dto">Objeto com os dados necessários para o cadastro.</param>
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
