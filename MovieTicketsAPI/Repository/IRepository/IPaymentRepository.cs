using MovieTicketsAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository.IRepository
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllPayments();
        Task<Payment> GetPaymentById(string id);
        Task<IEnumerable<Payment>> GetPaymentsByBookingId(string bookingId);
        Task CreatePayment(Payment payment);
        Task UpdatePayment(string id, Payment payment);
        Task DeletePayment(string id);
    }
}
