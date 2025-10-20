using System.Linq.Expressions;
using TurmaMaisA.Models;
using TurmaMaisA.Models.Students;
using TurmaMaisA.Persistence.Interfaces;
using TurmaMaisA.Services.Shared.Dtos;
using TurmaMaisA.Services.Students.Dtos;
using TurmaMaisA.Utils.Exceptions;
using TurmaMaisA.Utils.Formatters;
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

        public Task<StudentDto> CreateAsync(StudentCreateDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<StudentDto> CreateAsync(StudentCreateDto dto, Guid organizationId)
        {
            bool cpfIsValid = CpfValidator.Validate(dto.Cpf);
            if (!cpfIsValid)
                throw new BusinessRuleException("CPF fornecido é inválido.");

            dto.Cpf = CpfFormatter.Format(dto.Cpf);
            bool cpfExists = await _repository.AnyAsync(s => s.Cpf == dto.Cpf);
            if (cpfExists)
                throw new BusinessRuleException("CPF já cadastrado.");

            dto.Email = dto.Email.Trim().ToLower();
            bool emailExists = await _repository.AnyAsync(s => s.Email == dto.Email);
            if (emailExists)
                throw new BusinessRuleException("Email já cadastrado.");

            var studentCount = await _repository.CountWithIgnoreQueryFiltersAsync(s => s.OrganizationId == organizationId);
            var student = new Student()
            {
                Name = dto.Name,
                Cpf = dto.Cpf,
                RA = (studentCount + 1).ToString(),
                Email = dto.Email,
                OrganizationId = organizationId
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

        public async Task<PagedResultDto<StudentListDto>> GetPagedItemsAsync(PagedInputDto dto)
        {
            Expression<Func<Student, bool>>? searchExp = null;

            if (!string.IsNullOrEmpty(dto.Search))
            {
                var search = dto.Search.Trim().ToLower();
                searchExp = (s) =>
                s.Name.Trim().ToLower().Contains(search) ||
                s.RA.Trim().ToLower().Contains(search) ||
                s.Cpf.Trim().ToLower().Contains(search);
            }

            var result = await _repository.GetPagedItemsAsync(dto.PageNumber, dto.PageSize, searchExp);
            return new PagedResultDto<StudentListDto>(result.Items.Select(x => new StudentListDto(x)).ToList(), result.TotalCount);
        }

        public async Task<StudentDto> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id) ??
                throw new NotFoundException("Student", id);

            return new StudentDto(entity);
        }

        public async Task UpdateAsync(StudentUpdateDto dto)
        {
            dto.Email = dto.Email.Trim().ToLower();
            bool emailExists = await _repository.AnyAsync(s => s.Id != dto.Id && s.Email == dto.Email);
            if (emailExists)
                throw new BusinessRuleException("Email já cadastrado.");

            var entity = await _repository.GetByIdAsync(dto.Id) ??
                throw new NotFoundException("Student", dto.Id);

            entity.Name = dto.Name;
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
