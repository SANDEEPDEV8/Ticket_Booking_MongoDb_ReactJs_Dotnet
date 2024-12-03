using MovieTicketsAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository.IRepository
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllBookings();
        Task<IEnumerable<Booking>> GetPaginatedBookings(int pageNumber, int pageSize, string customerId = null);
        Task<IEnumerable<Booking>> GetBookingsByScheduleId(string scheduleId);
        Task<long> GetTotalBookingsCount();
        Task<Booking> GetBookingById(string id);
        Task<IEnumerable<Booking>> GetBookingsByCustomerId(string customerId);
        Task CreateBooking(Booking booking);
        Task UpdateBooking(string id, Booking booking);
        Task DeleteBooking(string id);
    }
}
