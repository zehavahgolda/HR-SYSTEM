using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class SystemModel 
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = null!;

    [BsonElement("requiredCapacity")]
    public int RequiredCapacity { get; set; }

    [BsonElement("isProject")]
    public bool IsProject { get; set; }

    // זה השדה שגרם לשגיאה - עכשיו הוא חלק מהממשק ומהמודל
    [BsonIgnore]
    public decimal ActualUtilization { get; set; }

    // שימוש ב-ActualUtilization לחישוב הפער
    [BsonIgnore]
    public decimal Gap => RequiredCapacity - ActualUtilization;
}