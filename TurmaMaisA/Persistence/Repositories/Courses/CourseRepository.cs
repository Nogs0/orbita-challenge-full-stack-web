using TurmaMaisA.Models;
using TurmaMaisA.Persistence.Repositories.Shared;

namespace TurmaMaisA.Persistence.Repositories.Courses
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(AppDbContext context)
            : base(context)
        { }
    }
}
