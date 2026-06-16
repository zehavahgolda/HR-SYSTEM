using HR_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace HR_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var employees = await _employeeService.GetAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpPut("{id}/allocation-months")]
        public async Task<IActionResult> UpdateAllocationMonths(string id, [FromQuery] string functionName, [FromQuery] int actualMonths)
        {
            var success = await _employeeService.UpdateActualMonthsAsync(id, functionName, actualMonths);

            if (!success)
                return NotFound("לא נמצא עובד או פונקציה תואמת לעדכון חודשי העבודה");

            return Ok(new { message = $"חודשי העבודה עבור {functionName} עודכנו ל-{actualMonths}" });
        }
    }
}