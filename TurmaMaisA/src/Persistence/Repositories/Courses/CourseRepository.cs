using Microsoft.EntityFrameworkCore;
using TurmaMaisA.Models.Courses;
using TurmaMaisA.Persistence.Repositories.Shared;

namespace TurmaMaisA.Persistence.Repositories.Courses
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        private readonly DbSet<Course> _courses;
        public CourseRepository(AppDbContext context)
            : base(context)
        { 
            _courses = context.Set<Course>();
        }

        public async Task<List<Course>> GetByIdsAsync(List<Guid> ids)
        {
            return await _courses.Where(c => ids.Contains(c.Id)).ToListAsync();
        }
    }
}
