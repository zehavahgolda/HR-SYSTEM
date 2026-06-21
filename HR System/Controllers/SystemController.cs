using Microsoft.AspNetCore.Mvc;
using HR_System.DTOs.Systems;
using HR_System.Services;

[ApiController]
[Route("api/[controller]")]
public class SystemController : ControllerBase
{
    private readonly ISystemService _systemService;

    public SystemController(ISystemService systemService)
    {
        _systemService = systemService;
    }

    [HttpGet]
    public async Task<ActionResult<List<SystemListItemDto>>> GetSystems(
        [FromQuery] int? year,
        [FromQuery] string? status,
        [FromQuery] string? ownerManagerName,
        [FromQuery] string? search)
    {
        try
        {
            var systems = await _systemService.GetSystemsAsync(year, status, ownerManagerName, search);
            return Ok(systems);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SystemDetailsDto>> GetSystemById(string id)
    {
        try
        {
            var system = await _systemService.GetSystemByIdAsync(id);
            if (system is null)
            {
                return NotFound();
            }

            return Ok(system);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}