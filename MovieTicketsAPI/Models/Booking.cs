using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MovieTicketsAPI.Models
{
    public class Booking
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("scheduleId")]
        public string ScheduleId { get; set; }

        [BsonElement("seatNumbers")]
        public List<int> SeatNumbers { get; set; } = new List<int>();

        [BsonElement("customerId")]
        public string CustomerId { get; set; }

        [BsonElement("numberOfSeats")]
        public int NumberOfSeats { get; set; }

        [BsonElement("totalPrice")]
        public decimal TotalPrice { get; set; }

        [BsonElement("bookingDate")]
        public DateTime BookingDate { get; set; }
    }
}
