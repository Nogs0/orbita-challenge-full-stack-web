using TurmaMaisA.Models.Enrollments;
using TurmaMaisA.Services.Courses.Dtos;
using TurmaMaisA.Services.Enrollments.Dtos;

namespace TurmaMaisA.Services.Enrollments
{
    public interface IEnrollmentService
    {
        Task<List<CourseDto>> GetByStudentIdAsync(Guid studentId);
        Task<List<Enrollment>> SetEnrolllmentsAsync(SetStudentEnrollmentsDto dto);
    }
}
