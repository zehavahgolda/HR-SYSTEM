using HR_System.Models;

namespace HR_System.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAsync();
    }
}