using TurmaMaisA.Models.Shared;

namespace TurmaMaisA.Models.Courses
{
    public interface ICourseRepository : IBaseRepository<Course>
    {
        Task<List<Course>> GetByIdsAsync(List<Guid> ids);
    }
}
