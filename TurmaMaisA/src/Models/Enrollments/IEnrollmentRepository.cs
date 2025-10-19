using TurmaMaisA.Models.Shared;

namespace TurmaMaisA.Models.Enrollments
{
    public interface IEnrollmentRepository : IBaseRepository<Enrollment>
    {
        Task<List<Enrollment>> GetAllByStudentId(Guid studentId);
        Task<List<Enrollment>> GetAllByCourseId(Guid courseId);
    }
}
