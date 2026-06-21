//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;

//public class SystemModel 
//{
//    [BsonId]
//    [BsonRepresentation(BsonType.ObjectId)]
//    public string? Id { get; set; }

//    [BsonElement("name")]
//    public string Name { get; set; } = null!;

//    [BsonElement("requiredCapacity")]
//    public int RequiredCapacity { get; set; }

//    [BsonElement("isProject")]
//    public bool IsProject { get; set; }

//    // זה השדה שגרם לשגיאה - עכשיו הוא חלק מהממשק ומהמודל
//    [BsonIgnore]
//    public decimal ActualUtilization { get; set; }

//    // שימוש ב-ActualUtilization לחישוב הפער
//    [BsonIgnore]
//    public decimal Gap => RequiredCapacity - ActualUtilization;
//}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HR_System.Models
{
    public class SystemModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } //מזהה מערכת 

        [BsonElement("name")]
        public string Name { get; set; } = null!; //שם מערכת 


        [BsonElement("ownerManagerName")]
        public string? OwnerManagerName { get; set; }//מנהל אחראי על מערכת - לשאול את חני האם יש אחד כזה. 

        [BsonElement("year")]
        public int Year { get; set; }//שנה

        [BsonElement("requiredCapacityMonths")]
        public int RequiredCapacityMonths { get; set; } //חודשי עבודה  מוקצים למערכת 

        [BsonElement("managementNote")]
        public string? ManagementNote { get; set; } // הערה ניהולית על המערכת 


        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true; // מערכת שאינה פעילה (לצורך מחיקה בעתיד, שמירת הסטוריה וכו
    }
}