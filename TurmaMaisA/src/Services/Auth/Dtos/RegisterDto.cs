using System.ComponentModel.DataAnnotations;

namespace TurmaMaisA.Services.Auth.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "The 'FullName' is required")]
        public required string FullName { get; set; }

        [Required(ErrorMessage = "The 'Email' is required.")]
        [EmailAddress(ErrorMessage = "The email format is invalid.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "The 'Password' is required.")]
        public required string Password { get; set; }

        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public required string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "The 'OrganizationName' is required")]
        public required string OrganizationName { get; set; }
    }
}
