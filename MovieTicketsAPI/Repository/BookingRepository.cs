using MongoDB.Driver;
using MovieTicketsAPI.Data;
using MovieTicketsAPI.Models;
using MovieTicketsAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly MongoDbContext _context;

        public BookingRepository(MongoDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<IEnumerable<Booking>> GetAllBookings()
        {
            return await _context.Bookings.Find(booking => true).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetPaginatedBookings(int pageNumber, int pageSize, string customerId = null)
        {
            FilterDefinition<Booking> filter;

            if (!string.IsNullOrEmpty(customerId))
            {
                // If customerId is provided, filter by CustomerId (for Users)
                filter = Builders<Booking>.Filter.Eq(b => b.CustomerId, customerId);
            }
            else
            {
                // If customerId is null, return all bookings (for Admin)
                filter = Builders<Booking>.Filter.Empty;
            }

            return await _context.Bookings
                                 .Find(filter)
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Limit(pageSize)
                                 .ToListAsync();
        }


        //public async Task<IEnumerable<Booking>> GetPaginatedBookings(int pageNumber, int pageSize)
        //{
        //    return await _context.Bookings
        //                         .Find(booking => true)
        //                         .Skip((pageNumber - 1) * pageSize)
        //                         .Limit(pageSize)
        //                         .ToListAsync();
        //}

        public async Task<IEnumerable<Booking>> GetBookingsByScheduleId(string scheduleId)
        {
            var filter = Builders<Booking>.Filter.Eq(b => b.ScheduleId, scheduleId);
            return await _context.Bookings.Find(filter).ToListAsync();
        }

        public async Task<long> GetTotalBookingsCount()
        {
            return await _context.Bookings.CountDocumentsAsync(booking => true);
        }


        public async Task<Booking> GetBookingById(string id)
        {
            return await _context.Bookings.Find(booking => booking.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByCustomerId(string customerId)
        {
            return await _context.Bookings.Find(booking => booking.CustomerId == customerId).ToListAsync();
        }

        public async Task CreateBooking(Booking booking)
        {
            await _context.Bookings.InsertOneAsync(booking);
        }

        public async Task UpdateBooking(string id, Booking booking)
        {
            await _context.Bookings.ReplaceOneAsync(b => b.Id == id, booking);
        }

        public async Task DeleteBooking(string id)
        {
            await _context.Bookings.DeleteOneAsync(booking => booking.Id == id);
        }
    }
}
