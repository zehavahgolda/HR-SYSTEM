using HR_System.Models;
using MongoDB.Driver;

namespace HR_System.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMongoCollection<Employee> _employeesCollection;

        public EmployeeService(IConfiguration config)
        {
            var mongoClient = new MongoClient(config["MongoDB:ConnectionString"]);
            var mongoDatabase = mongoClient.GetDatabase(config["MongoDB:DatabaseName"]);
            _employeesCollection = mongoDatabase.GetCollection<Employee>("Employees");
        }

        public async Task<List<Employee>> GetAsync() =>
            await _employeesCollection.Find(_ => true).ToListAsync();
    }
}