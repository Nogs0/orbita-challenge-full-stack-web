using System.ComponentModel.DataAnnotations;

namespace TurmaMaisA.Services.Enrollments.Dtos
{
    public class SetStudentEnrollmentsDto
    {
        [Required(ErrorMessage = "The 'StudentId' is required")]
        public required Guid StudentId { get; set; }
        
        [Required(ErrorMessage = "The 'CoursesIds' is required")]
        public required List<Guid> CoursesIds { get; set; }
    }
}
