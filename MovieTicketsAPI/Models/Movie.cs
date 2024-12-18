using Microsoft.AspNetCore.Http;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;
using System;

namespace MovieTicketsAPI.Models
{
    public class Movie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // MongoDB uses string ObjectId
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }
        //public string TheatreId { get; set; }

        [BsonElement("language")]
        public string Language { get; set; }

        [BsonElement("duration")]
        public double Duration { get; set; }
        //public DateTime PlayingDate { get; set; }
        //public DateTime PlayingTime { get; set; }
        //public double TicketPrice { get; set; }

        [BsonElement("rating")]
        public string Rating { get; set; }

        [BsonElement("genreId")]
        public string GenreId { get; set; }

        [BsonElement("actorName")]
        public string ActorName { get; set; }

        [BsonElement("directorName")]
        public string DirectorName { get; set; }


        [BsonElement("trailorUrl")]
        public string TrailorUrl { get; set; }

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; }
        [BsonIgnore] // Ignore this property in MongoDB
        public IFormFile Image { get; set; }
    }
}
