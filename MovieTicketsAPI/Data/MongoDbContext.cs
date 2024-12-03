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
            _database = client.GetDatabase("TicketDB");  // Replace with your database name
        }

        public IMongoCollection<User> Admins => _database.GetCollection<User>("Admins");
        public IMongoCollection<User> Customers => _database.GetCollection<User>("Customers");
        public IMongoCollection<Genre> Genres => _database.GetCollection<Genre>("Genres");
        public IMongoCollection<Theatre> Theatres => _database.GetCollection<Theatre>("Theatres");
        public IMongoCollection<Timeslot> Timeslots => _database.GetCollection<Timeslot>("Timeslots");
        public IMongoCollection<Schedule> Schedules => _database.GetCollection<Schedule>("Schedules");
        public IMongoCollection<Booking> Bookings => _database.GetCollection<Booking>("Bookings");
        public IMongoCollection<Payment> Payments => _database.GetCollection<Payment>("Payments");




        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Movie> Movies => _database.GetCollection<Movie>("Movies");
        public IMongoCollection<Reservation> Reservations => _database.GetCollection<Reservation>("Reservations");
        
    }
}
