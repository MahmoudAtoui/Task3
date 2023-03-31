using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Task3.Models;

public class Log
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    /*[BsonElement("Name")] public string LogName { get; set; }*/
    [BsonElement("RequestId")] public string RequestId { get; set; }
    [BsonElement("RequestBody")] public string RequestBody { get; set; }
    [BsonElement("RouteURL")] public string RouteURL { get; set; }
    [BsonElement("Timestamp")] public DateTime Timestamp { get; set; }

}