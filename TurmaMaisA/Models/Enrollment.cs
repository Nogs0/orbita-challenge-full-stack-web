using TurmaMaisA.Models.Shared;

namespace TurmaMaisA.Models
{
    public class Enrollment : BaseEntity
    {
        public Student? Student { get; set; }
        public Guid StudentId { get; set; }
        public Course? Course { get; set; }
        public Guid CourseId { get; set; }
    }
}
