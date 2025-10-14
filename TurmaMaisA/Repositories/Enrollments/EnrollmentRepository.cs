using TurmaMaisA.Models;
using TurmaMaisA.Persistence;
using TurmaMaisA.Repositories.Shared;

namespace TurmaMaisA.Repositories.Enrollments
{
    public class EnrollmentRepository : BaseRepository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(AppDbContext context)
            : base(context)
        { }
    }
}
