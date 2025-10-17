using System.ComponentModel.DataAnnotations;

namespace TurmaMaisA.Services.Courses.Dtos
{
    public class CourseCreateDto
    {
        [Required(ErrorMessage = "The field 'Name' is required")]
        [MaxLength(128)]
        public required string Name { get; set; }
    }
}
