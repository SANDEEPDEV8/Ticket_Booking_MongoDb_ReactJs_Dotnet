using Microsoft.AspNetCore.Http;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;
using System;

namespace MovieTicketsAPI.Models
{
    public class MyMovie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // MongoDB uses string ObjectId
        public string Name { get; set; }
        public string Description { get; set; }
        public string TheatreId { get; set; }
        public string Language { get; set; }
        public string Duration { get; set; }
        public DateTime PlayingDate { get; set; }
        public DateTime PlayingTime { get; set; }
        public double TicketPrice { get; set; }
        public double Rating { get; set; }
        public string Genre { get; set; }
        public string TrailorUrl { get; set; }
        public string ImageUrl { get; set; }
        [BsonIgnore] // Ignore this property in MongoDB
        public IFormFile Image { get; set; }
        [BsonIgnore]
        public ICollection<Reservation> Reservations { get; set; }
    }
}
