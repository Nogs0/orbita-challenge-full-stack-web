using TurmaMaisA.Models;
using TurmaMaisA.Persistence;
using TurmaMaisA.Repositories.Shared;

namespace TurmaMaisA.Repositories.Students
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(AppDbContext context)
            : base(context)
        { }
    }
}
