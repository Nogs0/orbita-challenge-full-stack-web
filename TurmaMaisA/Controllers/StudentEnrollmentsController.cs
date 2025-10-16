using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurmaMaisA.Services.Enrollments;
using TurmaMaisA.Services.Enrollments.Dtos;

namespace TurmaMaisA.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Students/{studentId}/[controller]")]
    public class StudentEnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _service;
        private readonly ILogger<StudentEnrollmentsController> _logger;
        public StudentEnrollmentsController(ILogger<StudentEnrollmentsController> logger,
            IEnrollmentService service)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPut]
        public async Task<IActionResult> SetEnrollments(Guid studentId, [FromBody] EnrollmentCreateDto dto)
        {
            if (studentId != dto.StudentId)
            {
                return BadRequest("O ID do estudante na rota não corresponde ao do corpo da requisição.");
            }

            _logger.LogInformation($"Creating enrollments to student with ID: {dto.StudentId}", dto.StudentId);
            await _service.SetEnrolllmentsAsync(dto);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetEnrollmentsByStudentId(Guid studentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation($"Fetching student's enrollments with ID: {studentId}", studentId);
            var enrollments = await _service.GetByStudentIdAsync(studentId);
            return Ok(enrollments);
        }
    }
}
