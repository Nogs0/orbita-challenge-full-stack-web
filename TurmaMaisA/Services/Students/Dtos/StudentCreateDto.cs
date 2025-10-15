using System.ComponentModel.DataAnnotations;

namespace TurmaMaisA.Services.Students.Dtos
{
    public class StudentCreateDto
    {

        [MaxLength(128)]
        public required string Name { get; set; }

        [MaxLength(128)]
        public required string Email { get; set; }

        [MaxLength(14)]
        public required string Cpf { get; set; }

        public Guid? OrganizationId { get; set; }
    }
}
