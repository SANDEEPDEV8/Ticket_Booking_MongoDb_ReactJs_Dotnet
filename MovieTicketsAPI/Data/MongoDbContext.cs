using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MovieTicketsAPI.Models;

namespace MovieTicketsAPI.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDbConnection"));
            _database = client.GetDatabase("MovieTicketDB");  // Replace with your database name
        }

        public IMongoCollection<MyUser> Users => _database.GetCollection<MyUser>("Users");
        public IMongoCollection<MyMovie> Movies => _database.GetCollection<MyMovie>("Movies");
        public IMongoCollection<MyReservation> Reservations => _database.GetCollection<MyReservation>("Reservations");
        public IMongoCollection<Theatre> Theatres => _database.GetCollection<Theatre>("Theatres");
    }
}
