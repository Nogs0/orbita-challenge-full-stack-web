using TurmaMaisA.Models;
using TurmaMaisA.Services.Shared;
using TurmaMaisA.Services.Shared.Dtos;
using TurmaMaisA.Services.Students.Dtos;

namespace TurmaMaisA.Services.Students
{
    public interface IStudentService : IBaseServiceCrud<Student, StudentDto, StudentListDto, StudentCreateDto, StudentUpdateDto>
    {
        Task<StudentDto> CreateAsync(StudentCreateDto dto, Guid organizationId);
        Task<PagedResultDto<StudentListDto>> GetPagedItemsAsync(PagedInputDto dto);
    }
}
