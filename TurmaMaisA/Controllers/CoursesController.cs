using Microsoft.AspNetCore.Mvc;
using TurmaMaisA.Services.Courses;
using TurmaMaisA.Services.Courses.Dtos;

namespace TurmaMaisA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _service;
        public CoursesController(ICourseService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseDto dto)
        {
            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CourseDto dto)
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok();
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok();
        }
    }
}
