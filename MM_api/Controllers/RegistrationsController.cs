using Microsoft.AspNetCore.Mvc;
using MM_api.DTOs;

namespace MM_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationService _service;

        public RegistrationsController(IRegistrationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpGet("{userId}/{marathonId}")]
        public async Task<IActionResult> GetByIdUserAndMarathon(int userId, int marathonId)
        {
            var result = await _service.GetByIdUserAndMarathonAsync(userId, marathonId);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRegistrationDTO dto)
        {
            await _service.CreateAsync(dto);
            return Ok(new { message = "Registration created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateRegistrationDTO dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok(new { message = "Registration updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _service.SoftDeleteAsync(id);
            return Ok(new { message = "Registration deleted successfully" });
        }
    }
}
