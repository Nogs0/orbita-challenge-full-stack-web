using TurmaMaisA.Models;
using TurmaMaisA.Services.Shared;
using TurmaMaisA.Services.Students.Dtos;

namespace TurmaMaisA.Services.Students
{
    public interface IStudentService : IBaseServiceCrud<Student, StudentDto, StudentListDto, StudentCreateDto, StudentUpdateDto>
    {
    }
}
