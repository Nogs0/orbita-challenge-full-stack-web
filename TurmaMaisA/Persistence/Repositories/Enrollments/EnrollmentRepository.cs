using TurmaMaisA.Models;
using TurmaMaisA.Persistence;
using TurmaMaisA.Persistence.Repositories.Shared;

namespace TurmaMaisA.Persistence.Repositories.Enrollments
{
    public class EnrollmentRepository : BaseRepository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(AppDbContext context)
            : base(context)
        { }
    }
}
