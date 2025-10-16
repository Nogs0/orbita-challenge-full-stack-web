using System.ComponentModel.DataAnnotations;

namespace TurmaMaisA.Services.Auth.Dtos
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "O usuário é obrigatório.")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public required string Password { get; set; }
    }
}
