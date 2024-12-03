using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieTicketsAPI.Models
{
    public class Timeslot
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("startTime")]
        public string StartTime { get; set; }  // e.g., "10:00 AM"

        [BsonElement("endTime")]
        public string EndTime { get; set; }    // e.g., "12:00 PM"
    }
}
