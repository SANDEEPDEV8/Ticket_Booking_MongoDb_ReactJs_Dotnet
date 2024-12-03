using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MovieTicketsAPI.Models
{
    public class Payment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("bookingId")]
        public string BookingId { get; set; }

        [BsonElement("amount")]
        public decimal Amount { get; set; }

        [BsonElement("paymentDate")]
        public DateTime PaymentDate { get; set; }

        [BsonElement("paymentMethod")]
        public string PaymentMethod { get; set; } // e.g., "Credit Card", "PayPal"
    }
}
