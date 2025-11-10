using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MM_api.DTOs;
using MM_api.DTOs.MM_api.DTOs;
using MM_api.Models;
using MM_api.Services;

namespace MM_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizerController : ControllerBase
    {
        private readonly IOrganizerService _service;
        private readonly IMapper _mapper;
        private readonly MarathonManagementV1Context _context;
        public OrganizerController(IOrganizerService service, IMapper mapper, MarathonManagementV1Context context)
        {
            _service = service;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("getId/{userId}")]
        public async Task<ActionResult<Organizer>> GetOrganizerIdByUserId(int userId)
        {
            var organizer = _context.Organizers.Any(o => o.UserId == userId && (o.IsDeleted == false || o.IsDeleted == null))
                ? await _service.GetByUserId(userId)
                : null;
            if (organizer == null)
                return NotFound("Organizer not found for this user.");

            return Ok(organizer); 
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadOrganizerDTO>>> GetAll()
        {
            var organizers = await _service.GetAllAsync();
            var dtoList = _mapper.Map<IEnumerable<ReadOrganizerDTO>>(organizers);
            return Ok(dtoList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReadOrganizerDTO>> GetById(int id)
        {
            var organizer = await _service.GetByIdAsync(id);
            if (organizer == null)
                return NotFound();

            var dto = _mapper.Map<ReadOrganizerDTO>(organizer);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrganizerDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var organizer = _mapper.Map<Organizer>(dto);
            await _service.AddAsync(organizer);

            var readDto = _mapper.Map<ReadOrganizerDTO>(organizer);
            return CreatedAtAction(nameof(GetById), new { id = organizer.OrganizerId }, readDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOrganizerDTO dto)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            _mapper.Map(dto, existing);
            await _service.UpdateAsync(existing);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
