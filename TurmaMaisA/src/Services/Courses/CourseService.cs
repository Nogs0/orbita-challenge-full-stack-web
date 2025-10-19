using System.Linq.Expressions;
using TurmaMaisA.Models.Courses;
using TurmaMaisA.Models.Enrollments;
using TurmaMaisA.Persistence.Interfaces;
using TurmaMaisA.Persistence.Repositories.Students;
using TurmaMaisA.Services.Courses.Dtos;
using TurmaMaisA.Services.Shared.Dtos;
using TurmaMaisA.Utils.Exceptions;

namespace TurmaMaisA.Services.Courses
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IUnitOfWork _uow;

        public CourseService(ICourseRepository repository, IUnitOfWork uow,
            IEnrollmentRepository enrollmentRepository)
        {
            _repository = repository;
            _enrollmentRepository = enrollmentRepository;
            _uow = uow;
        }

        public async Task<CourseDto> CreateAsync(CourseCreateDto dto)
        {
            var course = new Course()
            {
                Name = dto.Name
            };

            await _repository.CreateAsync(course);
            await _uow.SaveChangesAsync();

            return new CourseDto(course);
        }

        public async Task<IEnumerable<CourseDto>> GetAllAsync()
        {
            var courses = await _repository.GetAllAsync();
            return courses.Select(c => new CourseDto(c));
        }

        public async Task<PagedResultDto<CourseDto>> GetPagedItemsAsync(PagedInputDto dto)
        {
            Expression<Func<Course, bool>>? searchExp = null;

            if (!string.IsNullOrEmpty(dto.Search))
            {
                var search = dto.Search.Trim().ToLower();
                searchExp = (s) =>
                s.Name.Trim().ToLower().Contains(dto.Search);
            }

            var result = await _repository.GetPagedItemsAsync(dto.PageNumber, dto.PageSize, searchExp);
            return new PagedResultDto<CourseDto>(result.Items.Select(x => new CourseDto(x)).ToList(), result.TotalCount);
        }

        public async Task<CourseDto> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id) ??
                throw new NotFoundException("Course", id);

            return new CourseDto(entity);
        }

        public async Task UpdateAsync(CourseUpdateDto dto)
        {
            var entity = await _repository.GetByIdAsync(dto.Id) ??
                throw new NotFoundException("Course", dto.Id);

            entity.Name = dto.Name;

            _repository.Update(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id) ??
                throw new NotFoundException("Course", id);

            var enrrolments = await _enrollmentRepository.GetAllByCourseId(id);
            if (enrrolments.Count > 0)
                throw new BusinessRuleException("Não é possível excluir um curso que algum aluno esteja matrículado.");

            _repository.Delete(entity);
            await _uow.SaveChangesAsync();
        }
    }
}
