using Microsoft.AspNetCore.Mvc;
using HR_System.Models;
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
    public async Task<ActionResult<List<SystemModel>>> GetAllSystems()
    {
        try
        {
            var systems = await _systemService.GetSystemsWithUtilizationAsync();
            return Ok(systems);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    [HttpGet("gap-analysis")]
    public async Task<ActionResult<List<SystemModel>>> GetGapAnalysis()
    {
        var analysis = await _systemService.GetSystemsGapAnalysisAsync();
        return Ok(analysis);
    }

    [HttpGet("gap-analysis/{departmentId}")]
    public async Task<ActionResult<List<SystemModel>>> GetGapAnalysisByDept(string departmentId)
    {
        var analysis = await _systemService.GetSystemsGapAnalysisByDepartmentAsync(departmentId);
        return Ok(analysis);
    }
    [HttpGet("report/{departmentId}")]
    public async Task<ActionResult<List<object>>> GetDepartmentReport(string departmentId)
    {
        // קריאה לפונקציה החדשה שיצרנו ב-Service
        var report = await _systemService.GetDepartmentReportAsync(departmentId);

        // אם הדו"ח ריק, אפשר להחזיר NotFound או פשוט רשימה ריקה
        if (report == null)
        {
            return NotFound("Department not found or no data available.");
        }

        return Ok(report);
    }
}