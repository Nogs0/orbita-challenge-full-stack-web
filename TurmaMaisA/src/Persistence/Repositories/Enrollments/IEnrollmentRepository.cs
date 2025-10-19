using TurmaMaisA.Models;
using TurmaMaisA.Persistence.Repositories.Shared;

namespace TurmaMaisA.Persistence.Repositories.Enrollments
{
    public interface IEnrollmentRepository : IBaseRepository<Enrollment>
    {
        Task<List<Enrollment>> GetAllByStudentId(Guid studentId);
        Task<List<Enrollment>> GetAllByCourseId(Guid courseId);
    }
}
