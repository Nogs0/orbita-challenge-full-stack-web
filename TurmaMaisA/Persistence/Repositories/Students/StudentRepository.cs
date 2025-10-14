using TurmaMaisA.Models;
using TurmaMaisA.Persistence;
using TurmaMaisA.Persistence.Repositories.Shared;

namespace TurmaMaisA.Persistence.Repositories.Students
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(AppDbContext context)
            : base(context)
        { }
    }
}
