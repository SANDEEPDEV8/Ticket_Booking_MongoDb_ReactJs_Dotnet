﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MovieTicketsAPI.Models
{
    public class Genre
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }
}
