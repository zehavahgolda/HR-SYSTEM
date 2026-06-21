//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;

//namespace HR_System.Models
//{
//    public class Employee
//    {
//        [BsonId]
//        [BsonRepresentation(BsonType.ObjectId)]
//        public string? Id { get; set; }

//        [BsonElement("name")]
//        public string Name { get; set; } = null!;

//        [BsonElement("departmentId")]
//        public string DepartmentId { get; set; } = null!;

//        [BsonElement("allocations")]
//        public List<FunctionAllocation> Allocations { get; set; } = new();

//        [BsonElement("totalActualMonths")] 
//        public int TotalActualMonths { get; set; }

//        [BsonElement("year")] 
//        public int Year { get; set; }
//    }

//    public class FunctionAllocation
//    {
//        [BsonElement("functionName")]
//        public string FunctionName { get; set; } = null!;

//        [BsonElement("systemId")]

//        [BsonRepresentation(BsonType.ObjectId)]
//        public string SystemId { get; set; } = null!;

//        [BsonElement("plannedMonths")]
//        public int PlannedMonths { get; set; }

//        [BsonElement("actualMonths")]
//        public int ActualMonths { get; set; }
//    }
//}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HR_System.Models
{
    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } //מזהה עובד

        [BsonElement("fullName")]
        public string FullName { get; set; } = null!;//שם עובד

        [BsonElement("professionalCategory")]
        public string ProfessionalCategory { get; set; } = null!;//(קטגוריה מקצועית (..פיתוח,יישום 

        [BsonElement("professionalSubCategory")]
        public string? ProfessionalSubCategory { get; set; } //Beckend,Frontend..

        [BsonElement("managerName")]
        public string ManagerName { get; set; } = null!;//שם מנהל 

        [BsonElement("year")]
        public int Year { get; set; }//שנה

        [BsonElement("yearlyCapacityMonths")]
        public int YearlyCapacityMonths { get; set; } = 12; //חודשים מוקצים לשנה

        [BsonElement("upcomingEvent")]
        public string? UpcomingEvent { get; set; } //ארוע מגיע

        [BsonElement("notes")]
        public string? Notes { get; set; } //הערות

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true; //פעיל 

        [BsonElement("allocations")]
        public List<EmployeeAllocation> Allocations { get; set; } = new();//הקצאות 
    }

    public class EmployeeAllocation
    {
        [BsonElement("systemId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string SystemId { get; set; } = null!; //לאיזו מערכת משויך

        [BsonElement("roleInSystem")]
        public string RoleInSystem { get; set; } = null!;//תפקידו במערכת 

        [BsonElement("plannedMonths")]
        public int PlannedMonths { get; set; }//חודשים מתוכננים 

        [BsonElement("actualMonths")]
        public int ActualMonths { get; set; } //חודשים בפועל שביצע העבוד
    }
}