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
            var mongoDatabase = mongoClient.GetDatabase(config["MongoDB:DatabaseName"]); // זה ימשוך "temp"
            _employeesCollection = mongoDatabase.GetCollection<Employee>("employees");
        }

        public async Task<List<Employee>> GetAsync() =>
            await _employeesCollection.Find(_ => true).ToListAsync();

        public async Task<bool> UpdateActualMonthsAsync(string employeeId, string functionName, int newActualMonths)
        {
            var filter = Builders<Employee>.Filter.And(
                Builders<Employee>.Filter.Eq(e => e.Id, employeeId),
                Builders<Employee>.Filter.ElemMatch(e => e.Allocations, a => a.FunctionName == functionName)
            );

            var update = Builders<Employee>.Update.Set("allocations.$.actualMonths", newActualMonths);

            var result = await _employeesCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}