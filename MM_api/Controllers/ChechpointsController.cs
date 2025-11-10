using Microsoft.AspNetCore.Mvc;
using MM_api.DTOs;
using MM_api.Services;

[ApiController]
[Route("api/[controller]")]
public class CheckpointsController : ControllerBase
{
    private readonly ICheckpointService _service;

    public CheckpointsController(ICheckpointService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var checkpoints = await _service.GetAllAsync();
        return Ok(checkpoints);
    }

    [HttpGet("marathon/{marathonId}")]
    public async Task<IActionResult> GetByMarathonId(int marathonId)
    {
        var checkpoints = await _service.GetByMarathonIdAsync(marathonId);
        return Ok(checkpoints);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var checkpoint = await _service.GetByIdAsync(id);
        if (checkpoint == null) return NotFound();
        return Ok(checkpoint);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCheckpointDTO dto)
    {
        var created = await _service.CreateAsync(dto);
        return Ok(created);
    }
    [HttpPost("many")]
    public async Task<IActionResult> Create(List<CreateCheckpointDTO> dtos)
    {
        try
        {
            foreach (var dto in dtos)
            {
                await _service.CreateAsync(dto);
            }
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCheckpointDTO dto)
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