using System.ComponentModel.DataAnnotations;

namespace TurmaMaisA.Services.Students.Dtos
{
    public class StudentUpdateDto
    {
        public Guid Id { get; set; }

        [MaxLength(128)]
        public required string Name { get; set; }

        [MaxLength(128)]
        public required string Email { get; set; }
    }
}
