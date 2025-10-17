using System.ComponentModel.DataAnnotations;

namespace TurmaMaisA.Services.Courses.Dtos
{
    public class CourseUpdateDto
    {
        [Required(ErrorMessage = "The 'Id' is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The 'Name' is required")]
        [MaxLength(128)]
        public required string Name { get; set; }

    }
}
