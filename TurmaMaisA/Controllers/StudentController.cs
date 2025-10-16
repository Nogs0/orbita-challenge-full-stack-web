using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TurmaMaisA.Services.Students;
using TurmaMaisA.Services.Students.Dtos;

namespace TurmaMaisA.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;
        private readonly ILogger<StudentController> _logger;
        public StudentController(IStudentService service, ILogger<StudentController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating a new student.");
            var claimOrganizationId = HttpContext.User.FindFirst("OrganizationId");
            if (claimOrganizationId == null)
                return BadRequest("You must to be logged in to create a student.");

            dto.OrganizationId = Guid.Parse(claimOrganizationId.Value);
            var student = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] StudentUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("The route ID does not match the request body ID.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation($"Updating student with ID: {dto.Id}", id);
            await _service.UpdateAsync(dto);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation($"Fetching student with ID: {id}", id);
            var student = await _service.GetByIdAsync(id);

            if (student == null)
            {
                _logger.LogWarning($"Student with ID: {id} not found.", id);
                return NotFound();
            }

            return Ok(student);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all students.");
            var students = await _service.GetAllAsync();
            return Ok(students);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"Deleting student with ID: {id}", id);
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
