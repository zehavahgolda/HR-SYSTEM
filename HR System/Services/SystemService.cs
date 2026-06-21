using HR_System.DTOs.Systems;
using HR_System.Models;
using MongoDB.Driver;

namespace HR_System.Services
{
    public class SystemService : ISystemService
    {
        private readonly IMongoCollection<SystemModel> _systemsCollection;
        private readonly IMongoCollection<Employee> _employeesCollection;

        public SystemService(IMongoDatabase database)
        {
            _systemsCollection = database.GetCollection<SystemModel>("systems");
            _employeesCollection = database.GetCollection<Employee>("employees");
        }

    
        public async Task<List<SystemListItemDto>> GetSystemsAsync(
            int? year = null,
            string? status = null,
            string? ownerManagerName = null,
            string? search = null)
        {
            var systems = await _systemsCollection.Find(_ => true).ToListAsync();
            var employees = await _employeesCollection.Find(_ => true).ToListAsync();

            var filtered = systems.AsEnumerable();

            if (year.HasValue)
            {
                filtered = filtered.Where(s => s.Year == year.Value);
            }

            if (!string.IsNullOrWhiteSpace(ownerManagerName))
            {
                filtered = filtered.Where(s =>
                    string.Equals(s.OwnerManagerName, ownerManagerName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchTerm = search.Trim();
                filtered = filtered.Where(s =>
                    (!string.IsNullOrWhiteSpace(s.Name) && s.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(s.OwnerManagerName) && s.OwnerManagerName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(s.ManagementNote) && s.ManagementNote.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            var list = filtered
                .Select(s => MapToListItemDto(s, employees))
                .ToList();

            if (!string.IsNullOrWhiteSpace(status))
            {
                list = list
                    .Where(s => string.Equals(s.CapacityStatus, status, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return list
                .OrderBy(s => s.Name)
                .ToList();
        }

      
        public async Task<SystemDetailsDto?> GetSystemByIdAsync(string id)
        {
            var system = await _systemsCollection.Find(s => s.Id == id).FirstOrDefaultAsync();
            if (system is null)
            {
                return null;
            }

            var employees = await _employeesCollection.Find(_ => true).ToListAsync();

            var assignedEmployees = employees
                .SelectMany(e => (e.Allocations ?? [])
                    .Where(a => string.Equals(a.SystemId, id, StringComparison.OrdinalIgnoreCase))
                    .Select(a => new { Employee = e, Allocation = a }))
                .Select(x => new SystemAssignedEmployeeDto(
                    x.Employee.Id ?? string.Empty,
                    x.Employee.FullName,
                    x.Employee.ProfessionalCategory,
                    x.Employee.ProfessionalSubCategory,
                    x.Employee.ManagerName,
                    x.Allocation.RoleInSystem,
                    x.Allocation.PlannedMonths,
                    x.Allocation.ActualMonths,
                    GetEmployeeAvailabilityStatus(x.Employee)))
                .OrderBy(x => x.FullName)
                .ToList();

            var allocatedMonths = GetAllocatedMonthsBySystemId(employees, id);
            var gap = system.RequiredCapacityMonths - allocatedMonths;

            return new SystemDetailsDto(
                system.Id ?? string.Empty,
                system.Name,
                system.RequiredCapacityMonths,
                allocatedMonths,
                gap,
                GetCapacityStatus(gap),
                GetAssignedEmployeesCount(employees, id),
                system.ManagementNote,
                null, // TODO: Add system relevant changes field in model when available.
                assignedEmployees,
                []);
        }

      
        private static SystemListItemDto MapToListItemDto(SystemModel system, IEnumerable<Employee> employees)
        {
            var systemId = system.Id ?? string.Empty;
            var allocatedMonths = GetAllocatedMonthsBySystemId(employees, systemId);
            var gap = system.RequiredCapacityMonths - allocatedMonths;

            return new SystemListItemDto(
                systemId,
                system.Name,
                system.Year,
                system.RequiredCapacityMonths,
                allocatedMonths,
                gap,
                GetCapacityStatus(gap),
                GetAssignedEmployeesCount(employees, systemId),
                system.ManagementNote);
        }

       
        private static int GetAllocatedMonthsBySystemId(IEnumerable<Employee> employees, string systemId)
        {
            return employees
                .SelectMany(e => e.Allocations ?? [])
                .Where(a => string.Equals(a.SystemId, systemId, StringComparison.OrdinalIgnoreCase))
                .Sum(a => a.ActualMonths);
        }

       
        private static int GetAssignedEmployeesCount(IEnumerable<Employee> employees, string systemId)
        {
            return employees
                .Where(e => (e.Allocations ?? [])
                    .Any(a => string.Equals(a.SystemId, systemId, StringComparison.OrdinalIgnoreCase)))
                .Count();
        }

       
        private static string GetCapacityStatus(int gap)
        {
            if (gap > 0)
            {
                return "Shortage";
            }

            if (gap == 0)
            {
                return "Balanced";
            }

            return "Excess";
        }

     
        private static string GetEmployeeAvailabilityStatus(Employee employee)
        {
            var allocated = (employee.Allocations ?? []).Sum(a => a.ActualMonths);
            var remaining = employee.YearlyCapacityMonths - allocated;

            if (remaining > 0)
            {
                return "Available";
            }

            if (remaining == 0)
            {
                return "Balanced";
            }

            return "Overloaded";
        }
    }
}
