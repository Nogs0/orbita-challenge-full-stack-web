using TurmaMaisA.Models;
using TurmaMaisA.Services.Shared;
using TurmaMaisA.Services.Shared.Dtos;
using TurmaMaisA.Services.Students.Dtos;

namespace TurmaMaisA.Services.Students
{
    public interface IStudentService : IBaseServiceCrud<Student, StudentDto, StudentListDto, StudentCreateDto, StudentUpdateDto>
    {
        Task<PagedResultDto<StudentListDto>> GetPagedItemsAsync(PagedInputDto dto);
    }
}
