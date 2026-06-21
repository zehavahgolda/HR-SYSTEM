namespace HR_System.DTOs.Systems
{
    /// <summary>
    /// DTO for displaying full system details with assigned employees.
    /// Extends SystemListItemDto with additional fields.
    /// </summary>
    public record SystemDetailsDto(
        string Id,
        string Name,
        int RequiredCapacityMonths,
        int AllocatedMonths,
        int Gap,
        string CapacityStatus,
        int AssignedEmployeesCount,
        string? ManagementNote,
        string? RelevantChanges,
        List<SystemAssignedEmployeeDto> AssignedEmployees,
        List<SystemRelevantChangeDto> SistemRelevantChanges
    );
}
