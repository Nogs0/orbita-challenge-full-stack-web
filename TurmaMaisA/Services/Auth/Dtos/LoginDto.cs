using System.ComponentModel.DataAnnotations;

namespace TurmaMaisA.Services.Auth.Dtos
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "The 'Username' is required")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "The 'Password' is required")]
        public required string Password { get; set; }
    }
}
