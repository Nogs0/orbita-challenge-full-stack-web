using TurmaMaisA.Models;
using TurmaMaisA.Persistence.Interfaces;
using TurmaMaisA.Persistence.Repositories.Courses;
using TurmaMaisA.Persistence.Repositories.Students;
using TurmaMaisA.Services.Students.Dtos;
using TurmaMaisA.Utils.Exceptions;
using TurmaMaisA.Utils.Validators;

namespace TurmaMaisA.Services.Students
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;
        private readonly IUnitOfWork _uow;

        public StudentService(IStudentRepository repository, 
            IUnitOfWork uow
            )
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<StudentDto> CreateAsync(StudentCreateDto dto)
        {
            bool cpfIsValid = CpfValidator.Validate(dto.Cpf);
            if (!cpfIsValid)
                throw new BusinessRuleException("CPF fornecido é inválido.");

            var studentCount = await _repository.CountWithIgnoreQueryFiltersAsync(s => s.OrganizationId == dto.OrganizationId);
            var student = new Student()
            {
                Name = dto.Name,
                Cpf = dto.Cpf,
                RA = (studentCount + 1).ToString(),
                Email = dto.Email,
            };

            await _repository.CreateAsync(student);
            await _uow.SaveChangesAsync();

            return new StudentDto(student);
        }

        public async Task<IEnumerable<StudentListDto>> GetAllAsync()
        {
            var students = await _repository.GetAllAsync();
            return students.Select(s => new StudentListDto(s));
        }

        public async Task<StudentDto> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id) ??
                throw new NotFoundException("Student", id);

            return new StudentDto(entity);
        }

        public async Task UpdateAsync(StudentUpdateDto dto)
        {
            var entity = await _repository.GetByIdAsync(dto.Id) ??
                throw new NotFoundException("Student", dto.Id);

            entity.Name  = dto.Name;
            entity.Email = dto.Email;

            _repository.Update(entity);
            await _uow.SaveChangesAsync();  
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id) ??
                throw new NotFoundException("Student", id);

            _repository.Delete(entity);
            await _uow.SaveChangesAsync();
        }
    }
}
