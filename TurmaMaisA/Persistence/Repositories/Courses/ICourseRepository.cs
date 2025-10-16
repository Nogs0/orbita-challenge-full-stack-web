using TurmaMaisA.Models;
using TurmaMaisA.Persistence.Repositories.Shared;

namespace TurmaMaisA.Persistence.Repositories.Courses
{
    public interface ICourseRepository : IBaseRepository<Course>
    {
        Task<List<Course>> GetByIdsAsync(List<Guid> ids);
    }
}
