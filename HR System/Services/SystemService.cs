using HR_System.Models;
using HR_System.Services;
using MongoDB.Driver;
using System.Linq; // זה קריטי בשביל הפעולות האלו
using System.Collections.Generic;
public class SystemService : ISystemService
{
    private readonly IMongoCollection<SystemModel> _systems;
    private readonly IMongoCollection<Employee> _employees;

    public SystemService(IMongoDatabase database)
    {
        _systems = database.GetCollection<SystemModel>("systems");
        _employees = database.GetCollection<Employee>("employees");
    }

    public async Task<List<SystemModel>> GetSystemsWithUtilizationAsync()
    {
        // 1. שליפת כל המערכות
        var systems = await _systems.Find(_ => true).ToListAsync();

        // 2. שליפת כל העובדים כדי לסכום את העבודה שלהם
        var allEmployees = await _employees.Find(_ => true).ToListAsync();

        foreach (var system in systems)
        {
            // 3. לוגיקת החישוב: סכימת ActualMonths לפי ה-SystemId
            system.ActualUtilization = allEmployees
                .SelectMany(e => e.Allocations)
                .Where(a => a.SystemId == system.Id)
                .Sum(a => (decimal)a.ActualMonths);
        }

        return systems;
    }
    public async Task<List<SystemModel>> GetSystemsGapAnalysisAsync()
    {
        var systems = await _systems.Find(_ => true).ToListAsync();
        var allEmployees = await _employees.Find(_ => true).ToListAsync();

        foreach (var system in systems)
        {
            decimal totalActual = allEmployees
                .SelectMany(e => e.Allocations)
                .Where(a => a.SystemId?.ToString() == system.Id?.ToString())
                .Sum(a => a.ActualMonths);

            system.ActualUtilization = totalActual;
        }

        return systems;
    }
    public async Task<List<SystemModel>> GetSystemsGapAnalysisByDepartmentAsync(string departmentId)
    {
        var systems = await _systems.Find(_ => true).ToListAsync();

        var departmentEmployees = await _employees
            .Find(e => e.DepartmentId == departmentId)
            .ToListAsync();

        foreach (var system in systems)
        {
            decimal totalActual = departmentEmployees
                .SelectMany(e => e.Allocations)
                .Where(a => a.SystemId?.ToString() == system.Id?.ToString())
                .Sum(a => a.ActualMonths);

            system.ActualUtilization = totalActual;
        }

        return systems;
    }
    // 1. פונקציית סכימה בלבד (הכי חזקה ונקיה)
    public async Task<Dictionary<string, decimal>> GetActualUsageByDepartmentAsync(string departmentId)
    {
        var employees = await _employees.Find(e => e.DepartmentId == departmentId).ToListAsync();

        // נשתמש במילון פשוט
        var usageMap = new Dictionary<string, decimal>();

        foreach (var emp in employees)
        {
            if (emp.Allocations == null) continue;

            foreach (var alloc in emp.Allocations)
            {
                if (alloc.SystemId == null) continue;

                string idKey = alloc.SystemId.ToString();

                if (usageMap.ContainsKey(idKey))
                    usageMap[idKey] += (decimal)alloc.ActualMonths;
                else
                    usageMap[idKey] = (decimal)alloc.ActualMonths;
            }
        }

        return usageMap;
    }

    // 2. הפונקציה שמאחדת הכל לדו"ח (זה מה שיחזיר את הטבלה)
    public async Task<List<object>> GetDepartmentReportAsync(string departmentId)
    {
        var systems = await _systems.Find(_ => true).ToListAsync();
        var actualUsageMap = await GetActualUsageByDepartmentAsync(departmentId);

        return systems.Select(s => new {
            SystemName = s.Name,
            IsProject = s.IsProject,
            Budget = s.RequiredCapacity, // התקציב
            Actual = actualUsageMap.ContainsKey(s.Id.ToString()) ? actualUsageMap[s.Id.ToString()] : 0 // הסכימה
        }).Cast<object>().ToList();
    }
}