using MovieTicketsAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository
{
    public interface IMyReservationRepository
    {
        Task<bool> CreateReservationAsync(MyReservation reservation);
        Task DeleteReservationAsync(string id);
        Task<IEnumerable<MyReservation>> GetAllReservations();
        Task<MyReservation> GetReservationById(string id);
        Task<IEnumerable<ReservationResponse>> GetReservationsAsync(string customerName, string sort, int pageNumber, int pageSize);
        Task<object> GetReservationDetailAsync(string id);
        Task UpdateReservationAsync(MyReservation reservation);
        Task<long> GetTotalCountAsync(string customerName);
    }
}