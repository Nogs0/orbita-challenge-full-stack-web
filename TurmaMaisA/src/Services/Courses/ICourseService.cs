using TurmaMaisA.Models;
using TurmaMaisA.Services.Courses.Dtos;
using TurmaMaisA.Services.Shared;
using TurmaMaisA.Services.Shared.Dtos;

namespace TurmaMaisA.Services.Courses
{
    public interface ICourseService : IBaseServiceCrud<Course, CourseDto, CourseDto, CourseCreateDto, CourseUpdateDto>
    {
        Task<PagedResultDto<CourseDto>> GetPagedItemsAsync(PagedInputDto dto);
    }
}
