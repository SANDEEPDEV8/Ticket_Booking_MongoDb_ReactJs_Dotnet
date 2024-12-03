using MongoDB.Driver;
using MovieTicketsAPI.Data;
using MovieTicketsAPI.Models;
using MovieTicketsAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly MongoDbContext _context;

        public PaymentRepository(MongoDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<IEnumerable<Payment>> GetAllPayments()
        {
            return await _context.Payments.Find(payment => true).ToListAsync();
        }

        public async Task<Payment> GetPaymentById(string id)
        {
            return await _context.Payments.Find(payment => payment.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByBookingId(string bookingId)
        {
            return await _context.Payments.Find(payment => payment.BookingId == bookingId).ToListAsync();
        }

        public async Task CreatePayment(Payment payment)
        {
            await _context.Payments.InsertOneAsync(payment);
        }

        public async Task UpdatePayment(string id, Payment payment)
        {
            await _context.Payments.ReplaceOneAsync(p => p.Id == id, payment);
        }

        public async Task DeletePayment(string id)
        {
            await _context.Payments.DeleteOneAsync(payment => payment.Id == id);
        }
    }
}
