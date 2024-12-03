using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieTicketsAPI.Models
{
    public class Schedule
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("date")]
        public string Date { get; set; } // e.g., "2023-12-01"

        //[BsonElement("seatNumber")]
        //public int SeatNumber { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("movieId")]
        public string MovieId { get; set; }

        [BsonElement("theatreId")]
        public string TheatreId { get; set; }

        [BsonElement("timeSlotId")]
        public string TimeSlotId { get; set; }
    }
}
