using HR_System.Models;

namespace HR_System.Services
{
    public interface ISystemService
    {
        Task<List<SystemModel>> GetSystemsWithUtilizationAsync();
        Task<List<SystemModel>> GetSystemsGapAnalysisAsync();
        Task<List<SystemModel>> GetSystemsGapAnalysisByDepartmentAsync(string departmentId);
        Task<List<object>> GetDepartmentReportAsync(string departmentId);
    }
}
