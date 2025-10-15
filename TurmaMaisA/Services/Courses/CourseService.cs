using TurmaMaisA.Models;
using TurmaMaisA.Persistence.Interfaces;
using TurmaMaisA.Persistence.Repositories.Courses;
using TurmaMaisA.Services.Courses.Dtos;

namespace TurmaMaisA.Services.Courses
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repository;
        private readonly IUnitOfWork _uow;

        public CourseService(ICourseRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<CourseDto> CreateAsync(CourseDto dto)
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

        public async Task<CourseDto> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id) ??
                throw new Exception($"Course with id: {id} not found.");

            return new CourseDto(entity);
        }

        public async Task UpdateAsync(CourseDto dto)
        {
            var entity = await _repository.GetByIdAsync(dto.Id) ??
                throw new Exception($"Course with id: {dto.Id} not found.");

            entity.Name = dto.Name;

            _repository.Update(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id) ??
                throw new Exception($"Course with id: {id} not found.");

            _repository.Delete(entity);
            await _uow.SaveChangesAsync();
        }
    }
}
