using Microsoft.EntityFrameworkCore;
using TurmaMaisA.Models;
using TurmaMaisA.Models.Students;
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

        public override async Task CreateAsync(Student entity)
        {
            var exists = await _students.IgnoreQueryFilters()
                .Where(s => s.OrganizationId == entity.OrganizationId)
                .FirstOrDefaultAsync(s => s.Cpf == entity.Cpf);

            if (exists == null)
                await base.CreateAsync(entity);
            else
            {
                exists.Name = entity.Name;
                exists.Email = entity.Email;
                exists.DeletedAt = null;
            }
        }

        public override async Task<Student?> GetByIdAsync(Guid id)
        {
            return await _students.Include(x => x.Enrollments).FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
