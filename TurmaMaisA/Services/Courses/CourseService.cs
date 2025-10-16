using Microsoft.AspNetCore.Http.HttpResults;
using TurmaMaisA.Models;
using TurmaMaisA.Persistence.Interfaces;
using TurmaMaisA.Persistence.Repositories.Courses;
using TurmaMaisA.Services.Courses.Dtos;
using TurmaMaisA.Utils.Exceptions;

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

        public async Task<CourseDto> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id) ??
                throw new NotFoundException("Course", id);

            return new CourseDto(entity);
        }

        public async Task UpdateAsync(CourseDto dto)
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

            _repository.Delete(entity);
            await _uow.SaveChangesAsync();
        }
    }
}
