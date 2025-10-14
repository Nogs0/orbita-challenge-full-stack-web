using Microsoft.AspNetCore.Mvc;
using TurmaMaisA.Services.Students;
using TurmaMaisA.Services.Students.Dtos;

namespace TurmaMaisA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;
        public StudentController(IStudentService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentDto dto)
        {
            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] StudentDto dto)
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
