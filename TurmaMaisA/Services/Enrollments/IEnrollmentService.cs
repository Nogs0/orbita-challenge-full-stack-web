using TurmaMaisA.Services.Enrollments.Dtos;

namespace TurmaMaisA.Services.Enrollments
{
    public interface IEnrollmentService
    {
        Task<List<EnrollmentDto>> GetByStudentIdAsync(Guid studentId);
        Task SetEnrolllmentsAsync(EnrollmentCreateDto dto);
    }
}
