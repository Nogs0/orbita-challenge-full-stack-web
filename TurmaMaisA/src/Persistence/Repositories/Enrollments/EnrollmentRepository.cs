using Microsoft.EntityFrameworkCore;
using TurmaMaisA.Models.Enrollments;
using TurmaMaisA.Persistence;
using TurmaMaisA.Persistence.Repositories.Shared;

namespace TurmaMaisA.Persistence.Repositories.Enrollments
{
    public class EnrollmentRepository : BaseRepository<Enrollment>, IEnrollmentRepository
    {
        private readonly DbSet<Enrollment> _enrollments;
        public EnrollmentRepository(AppDbContext context)
            : base(context)
        {
            _enrollments = context.Set<Enrollment>();
        }

        public async Task<List<Enrollment>> GetAllByStudentId(Guid studentId)
        {
            return await _enrollments.Include(e => e.Course)
                .Where(x => x.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<List<Enrollment>> GetAllByCourseId(Guid courseId)
        {
            return await _enrollments.Include(e => e.Course)
                .Where(x => x.CourseId == courseId)
                .ToListAsync();
        }
    }
}
