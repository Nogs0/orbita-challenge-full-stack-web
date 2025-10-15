using System.ComponentModel.DataAnnotations;

namespace TurmaMaisA.Services.Auth.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        public required string FullName { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O formato do email é inválido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public required string Password { get; set; }

        [Compare("Password", ErrorMessage = "As senhas não conferem.")]
        public required string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "O nome da organização é obrigatório.")]
        public required string OrganizationName { get; set; }
    }
}
