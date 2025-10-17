using System.ComponentModel.DataAnnotations;

namespace TurmaMaisA.Services.Students.Dtos
{
    public class StudentCreateDto
    {
        [Required(ErrorMessage = "The 'Name' is required")]
        [MaxLength(128)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "The 'Email' is required")]
        [MaxLength(128)]
        public required string Email { get; set; }

        [Required(ErrorMessage = "The 'Cpf' is required")]
        [MaxLength(14)]
        public required string Cpf { get; set; }

        public Guid? OrganizationId { get; set; }
    }
}
