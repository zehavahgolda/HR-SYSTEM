using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HR_System.Models
{
    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("departmentId")]
        public string DepartmentId { get; set; } = null!;

        [BsonElement("allocations")]
        public List<FunctionAllocation> Allocations { get; set; } = new();

        [BsonElement("totalActualMonths")] 
        public int TotalActualMonths { get; set; }

        [BsonElement("year")] 
        public int Year { get; set; }
    }

    public class FunctionAllocation
    {
        [BsonElement("functionName")]
        public string FunctionName { get; set; } = null!;

        [BsonElement("systemId")]

        [BsonRepresentation(BsonType.ObjectId)]
        public string SystemId { get; set; } = null!;

        [BsonElement("plannedMonths")]
        public int PlannedMonths { get; set; }

        [BsonElement("actualMonths")]
        public int ActualMonths { get; set; }
    }
}