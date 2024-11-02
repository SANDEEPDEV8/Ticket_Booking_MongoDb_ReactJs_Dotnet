using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MovieTicketsAPI.Data;
using MovieTicketsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository
{
    public class MyReservationRepository : IMyReservationRepository
    {
        private MongoDbContext _context;
        public MyReservationRepository(MongoDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<IEnumerable<MyReservation>> GetAllReservations()
        {
            return await _context.Reservations.Find(reservation => true).ToListAsync();
        }


        public async Task<MyReservation> GetReservationById(string id)
        {
            return await _context.Reservations.Find(reservation => reservation.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MyReservation>> GetReservationsById(string userId)
        {
            return await _context.Reservations.Find(reservation => reservation.UserId == userId).ToListAsync();
        }


        public async Task<IEnumerable<ReservationResponse>> GetReservationsAsync(string customerName, string sort, int pageNumber, int pageSize)
        {
            var sortDefinitionBuilder = Builders<MyReservation>.Sort;
            SortDefinition<MyReservation> sortDefinition;
            switch (sort)
            {
                case "highest_price":
                    sortDefinition = sortDefinitionBuilder.Descending(m => m.Price);
                    break;
                case "lowest_price":
                    sortDefinition = sortDefinitionBuilder.Ascending(m => m.Price);
                    break;
                case "latest_reservation":
                    sortDefinition = sortDefinitionBuilder.Descending(m => m.ReservationTime);
                    break;
                case "earliest_reservation":
                    sortDefinition = sortDefinitionBuilder.Ascending(m => m.ReservationTime);
                    break;
                default:
                    sortDefinition = sortDefinitionBuilder.Descending(m => m.ReservationTime);
                    break;
            }

            //var filter = Builders<MyReservation>.Filter.Regex("Name", new MongoDB.Bson.BsonRegularExpression($"^{customerName}", "i"));

            var filter = Builders<MyUser>.Filter.Regex("Name", new MongoDB.Bson.BsonRegularExpression(customerName, "i"));
            var users = await _context.Users.Find(x => filter.Inject()).ToListAsync();
            var userIds = users.Select(u => u.Id).ToList();

            // get the corresponding reservations of this users with pagination
            var reservationFilter = Builders<MyReservation>.Filter.In(r => r.UserId, userIds);
            var reservations = await _context.Reservations.Find(reservationFilter)
                                        .Sort(sortDefinition)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Limit(pageSize)
                                        .ToListAsync();


            // get the corresponding movies of this reservations
            var movieIds = reservations.Select(r => r.MovieId).ToList();
            var movieFilter = Builders<MyMovie>.Filter.In(m => m.Id, movieIds);
            var movies = await _context.Movies.Find(movieFilter).ToListAsync();


            //now build my reservation object array
            var myReservations = new List<ReservationResponse>();
            foreach (var reservation in reservations)
            {
                var user = users.FirstOrDefault(u => u.Id == reservation.UserId);
                var movie = movies.FirstOrDefault(m => m.Id == reservation.MovieId);
                myReservations.Add(new ReservationResponse
                {
                    Id = reservation.Id,
                    CustomerName = user.Name,
                    MovieName = movie.Name,
                    MovieImageUrl = movie.ImageUrl,
                    ReservationTime = reservation.ReservationTime

                    //UserId = reservation.UserId,
                    //MovieId = reservation.MovieId,
                    //Qty = reservation.Qty,
                    //Price = reservation.Price,
                    //Phone = reservation.Phone,
                });
            }

            return myReservations;
            //var reservationFilter = Builders<MyReservation>.Filter.In(r => r.UserId, userIds);
            //var count = await _context.Reservations.CountDocumentsAsync(reservationFilter);

            //return await _context.Reservations.Find(reservation => filter.Inject())
            //                            .Sort(sortDefinition)
            //                            .Skip((pageNumber - 1) * pageSize)
            //                            .Limit(pageSize)
            //                            .ToListAsync();
        }

    
        public async Task<object> GetReservationDetailAsync(string id)
        {
            var reservation = await _context.Reservations.Find(r => r.Id == id).FirstOrDefaultAsync();
            var user = await _context.Users.Find(u => u.Id == reservation.UserId).FirstOrDefaultAsync();
            var movie = await _context.Movies.Find(m => m.Id == reservation.MovieId).FirstOrDefaultAsync();

            return new 
            {
                Id = reservation.Id,
                ReservationTime = reservation.ReservationTime,
                CustomerName = user.Name,
                MovieName = movie.Name,
                MovieImageUrl = movie.ImageUrl,
                Email = user.Email,
                Qty = reservation.Qty,
                Price = reservation.Price,
                Phone = reservation.Phone,
                PlayingDate = movie.PlayingDate,
                PlayingTime = movie.PlayingTime
            };
        }

        private int FindNextNumberToFill(List<int> inputList, int total)
        {
            // Create a set of all numbers from 1 to total
            HashSet<int> fullSet = new HashSet<int>(Enumerable.Range(1, total));

            // Remove numbers from the set that are present in the input list
            foreach (int num in inputList)
            {
                fullSet.Remove(num);
            }

            // Return the smallest missing number
            return fullSet.Min();
        }

        public async Task<bool> CreateReservationAsync(MyReservation reservation)
        {
            var userReservations = await GetReservationsById(reservation.UserId);
            var filledSeats = userReservations.Select(x => x.SeatNo).ToList();
            int capacity = 5;
            if (filledSeats.Count == capacity)
            {
                return false;
            }
            int availableSeat = FindNextNumberToFill(filledSeats, capacity);

            reservation.SeatNo = availableSeat;

            await _context.Reservations.InsertOneAsync(reservation);

            return true;
        }

        public async Task UpdateReservationAsync(MyReservation reservation)
        {
            await _context.Reservations.ReplaceOneAsync(r => r.Id == reservation.Id, reservation);
        }

        public async Task DeleteReservationAsync(string id)
        {
            await _context.Reservations.DeleteOneAsync(r => r.Id == id);
        }

        public async Task<long> GetTotalCountAsync(string customerName)
        {
            var filter = Builders<MyUser>.Filter.Regex("Name", new MongoDB.Bson.BsonRegularExpression(customerName, "i"));
            var users = await _context.Users.Find(x => filter.Inject()).ToListAsync();
            var userIds = users.Select(u => u.Id).ToList();

            var reservationFilter = Builders<MyReservation>.Filter.In(r => r.UserId, userIds);
            var count = await _context.Reservations.CountDocumentsAsync(reservationFilter);

            return (long)count;

        }

    }
}
