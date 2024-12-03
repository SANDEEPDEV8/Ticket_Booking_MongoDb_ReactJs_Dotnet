using MovieTicketsAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository.IRepository
{
    public interface IReservationRepository
    {
        Task<bool> CreateReservationAsync(Reservation reservation);
        Task DeleteReservationAsync(string id);
        Task<IEnumerable<Reservation>> GetAllReservations();
        Task<Reservation> GetReservationById(string id);
        Task<IEnumerable<ReservationResponse>> GetReservationsAsync(string customerName, string sort, int pageNumber, int pageSize);
        Task<object> GetReservationDetailAsync(string id);
        Task UpdateReservationAsync(Reservation reservation);
        Task<long> GetTotalCountAsync(string customerName);
    }
}