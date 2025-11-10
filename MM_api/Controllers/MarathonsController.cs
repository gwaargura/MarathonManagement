using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MM_api.DTOs;
using MM_api.DTOs.MM_api.DTOs;
using MM_api.Services;
using System.Threading.Tasks;

namespace MM_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarathonsController : ControllerBase
    {
        private readonly IMarathonService _service;

        public MarathonsController(IMarathonService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var marathons = await _service.GetAllAsync();
            return Ok(marathons);
        }

        [HttpGet("organizer/{id}")]
        public async Task<IActionResult> GetOrganizerAll(int id)
        {
            var marathons = await _service.GetOrganizerAsync(id);
            return Ok(marathons);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var marathon = await _service.GetByIdAsync(id);
            if (marathon == null) return NotFound();
            return Ok(marathon);
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var marathon = await _service.GetMarathonDetailsAsync(id);
            if (marathon == null) return NotFound();
            return Ok(marathon);
        }

        [Authorize(Roles = "Organizer")]
        [HttpPost]
        public async Task<IActionResult> CreateMarathon(CreateMarathonDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            var marathon = await _service.GetByIdAsync(created.MarathonId);
            return Created("", marathon);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateMarathonDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var deleted = await _service.SoftDeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
