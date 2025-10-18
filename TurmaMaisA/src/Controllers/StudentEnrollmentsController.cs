using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurmaMaisA.Services.Enrollments;
using TurmaMaisA.Services.Enrollments.Dtos;

namespace TurmaMaisA.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Students/{studentId}/Enrollments")]
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

        /// <summary>
        /// Define ou sincroniza a lista de matrículas para um estudante específico.
        /// </summary>
        /// <param name="studentId">O ID do estudante cujas matrículas serão definidas.</param>
        /// <param name="dto">Objeto contendo o ID do estudante e a lista completa dos IDs dos cursos.</param>
        [HttpPut]
        public async Task<IActionResult> SetEnrollments(Guid studentId, [FromBody] SetStudentEnrollmentsDto dto)
        {
            if (studentId != dto.StudentId)
            {
                return BadRequest("O ID do estudante na rota não corresponde ao do corpo da requisição.");
            }

            _logger.LogInformation($"Creating enrollments to student with ID: {dto.StudentId}", dto.StudentId);
            var enrollments = await _service.SetEnrolllmentsAsync(dto);
            var result = enrollments.Select(e => new EnrollmentDto(e)).ToList();
            return Ok(result);
        }

        /// <summary>
        /// Busca todas as matrículas associadas a um estudante específico.
        /// </summary>
        /// <param name="studentId">O ID do estudante para o qual as matrículas serão buscadas.</param>
        [HttpGet]
        public async Task<IActionResult> GetEnrollmentsByStudentId(Guid studentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation($"Fetching student's enrollments with ID: {studentId}", studentId);
            var courses = await _service.GetByStudentIdAsync(studentId);
            return Ok(courses);
        }
    }
}
