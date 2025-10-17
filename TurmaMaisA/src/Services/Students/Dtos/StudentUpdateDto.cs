using System.ComponentModel.DataAnnotations;

namespace TurmaMaisA.Services.Students.Dtos
{
    public class StudentUpdateDto
    {
        [Required(ErrorMessage = "The 'Id' is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The 'Name' is required")]
        [MaxLength(128)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "The 'Email' is required")]
        [MaxLength(128)]
        public required string Email { get; set; }
    }
}
