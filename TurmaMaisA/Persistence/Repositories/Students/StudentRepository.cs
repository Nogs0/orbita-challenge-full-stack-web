using Microsoft.EntityFrameworkCore;
using TurmaMaisA.Models;
using TurmaMaisA.Persistence;
using TurmaMaisA.Persistence.Repositories.Shared;

namespace TurmaMaisA.Persistence.Repositories.Students
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        private DbSet<Student> _students;
        public StudentRepository(AppDbContext context)
            : base(context)
        {
            _students = context.Set<Student>();
        }

        public override async Task<Student?> GetByIdAsync(Guid id)
        {
            return await _students.Include(x => x.Enrollments).FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
