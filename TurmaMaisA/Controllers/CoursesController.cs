using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurmaMaisA.Services.Courses;
using TurmaMaisA.Services.Courses.Dtos;

namespace TurmaMaisA.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _service;
        private readonly ILogger<CoursesController> _logger;
        public CoursesController(ICourseService service, ILogger<CoursesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating a new course.");
            var course = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CourseDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("The route ID does not match the request body ID.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation($"Updating course with ID: {dto.Id}", id);
            await _service.UpdateAsync(dto);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation($"Fetching course with ID: {id}", id);
            var course = await _service.GetByIdAsync(id);

            if (course == null)
            {
                _logger.LogWarning($"Course with ID: {id} not found.", id);
                return NotFound();
            }

            return Ok(course);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all courses.");
            var courses = await _service.GetAllAsync();
            return Ok(courses);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"Deleting course with ID: {id}", id);
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
