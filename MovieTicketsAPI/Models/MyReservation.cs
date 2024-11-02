using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace MovieTicketsAPI.Models
{
    public class MyReservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int Qty { get; set; }
        public int SeatNo { get; set; }
        public double Price { get; set; }
        public string Phone { get; set; }
        public DateTime ReservationTime { get; set; }
        public string MovieId { get; set; }  // Foreign key reference
        public string UserId { get; set; }  // Foreign key reference
    }
}
