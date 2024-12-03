using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace MovieTicketsAPI.Models
{
    public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("qty")]
        public int Qty { get; set; }

        [BsonElement("seatNo")]
        public int SeatNo { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("email")]
        public string Phone { get; set; }

        [BsonElement("reservationDate")]
        public DateTime ReservationTime { get; set; }

        [BsonElement("movieId")]
        public string MovieId { get; set; }  // Foreign key reference

        [BsonElement("userId")]
        public string UserId { get; set; }  // Foreign key reference
    }
}
