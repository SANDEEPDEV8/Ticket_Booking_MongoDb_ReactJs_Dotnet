using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieTicketsAPI.Models
{
    public class Theatre
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("screenNumber")]
        public int ScreenNumber { get; set; }

        [BsonElement("location")]
        public string Location { get; set; }

        [BsonElement("seatCapacity")]
        public int SeatCapacity { get; set; }
    }
}
