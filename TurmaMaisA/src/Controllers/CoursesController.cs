using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurmaMaisA.Services.Courses;
using TurmaMaisA.Services.Courses.Dtos;
using TurmaMaisA.Services.Shared.Dtos;

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

        /// <summary>
        /// Cria um novo curso no sistema.
        /// </summary>
        /// <param name="dto">Objeto contendo os dados necessários para a criação do curso.</param>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating a new course.");
            var course = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
        }

        /// <summary>
        /// Atualiza as informações de um curso existente.
        /// </summary>
        /// <param name="id">O ID do curso a ser atualizado.</param>
        /// <param name="dto">Objeto contendo os dados atualizados do curso.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CourseUpdateDto dto)
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

        /// <summary>
        /// Busca um curso específico pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do curso a ser buscado.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation($"Fetching course with ID: {id}", id);
            var course = await _service.GetByIdAsync(id);
            return Ok(course);
        }

        /// <summary>
        /// Retorna uma lista paginada com todos os cursos cadastrados.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] PagedInputDto dto)
        {
            _logger.LogInformation("Fetching courses.");
            var courses = await _service.GetPagedItemsAsync(dto);
            return Ok(courses);
        }

        /// <summary>
        /// Retorna uma lista paginada com todos os cursos cadastrados.
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching courses.");
            var courses = await _service.GetAllAsync();
            return Ok(courses);
        }

        /// <summary>
        /// Exclui um curso do sistema.
        /// </summary>
        /// <param name="id">O ID do curso a ser excluído.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"Deleting course with ID: {id}", id);
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
