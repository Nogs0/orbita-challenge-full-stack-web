using TurmaMaisA.Models;
using TurmaMaisA.Services.Courses.Dtos;
using TurmaMaisA.Services.Shared;

namespace TurmaMaisA.Services.Courses
{
    public interface ICourseService : IBaseServiceCrud<Course, CourseDto, CourseDto, CourseDto, CourseDto>
    {
    }
}
