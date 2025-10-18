using TurmaMaisA.Models;
using TurmaMaisA.Services.Courses.Dtos;

namespace TurmaMaisA.Services.Enrollments.Dtos
{
    public class EnrollmentDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }

        public EnrollmentDto() { }

        public EnrollmentDto(Enrollment entity)
        {
            Id = entity.Id;
            StudentId = entity.StudentId;
            CourseId = entity.CourseId;
            if (entity.Course != null)
                CourseName = entity.Course.Name;
        }
    }
}
