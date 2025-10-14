using TurmaMaisA.Models;
using TurmaMaisA.Persistence;
using TurmaMaisA.Repositories.Classes;
using TurmaMaisA.Repositories.Shared;

namespace TurmaMaisA.Repositories.Courses
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(AppDbContext context)
            : base(context)
        { }
    }
}
