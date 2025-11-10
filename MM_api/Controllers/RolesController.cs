using Microsoft.AspNetCore.Mvc;
using MM_api.Services;
using MM_api.DTOs;
using MM_api.Models;
using AutoMapper;

namespace MM_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _service;
        private readonly IMapper _mapper;

        public RolesController(IRoleService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadRoleDTO>>> GetAll()
        {
            var roles = await _service.GetAllAsync();
            var dtoList = _mapper.Map<IEnumerable<ReadRoleDTO>>(roles);
            return Ok(dtoList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReadRoleDTO>> GetById(int id)
        {
            var role = await _service.GetByIdAsync(id);
            if (role == null)
                return NotFound();

            var dto = _mapper.Map<ReadRoleDTO>(role);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var role = _mapper.Map<Role>(dto);
            await _service.AddAsync(role);

            var readDto = _mapper.Map<ReadRoleDTO>(role);
            return CreatedAtAction(nameof(GetById), new { id = role.RoleId }, readDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleDTO dto)
        {
            if (id != 1 || id != 2 || id != 3)
                return BadRequest("Role ID mismatch");

            var existingRole = await _service.GetByIdAsync(id);
            if (existingRole == null)
                return NotFound();

            _mapper.Map(dto, existingRole);
            await _service.UpdateAsync(existingRole);

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
