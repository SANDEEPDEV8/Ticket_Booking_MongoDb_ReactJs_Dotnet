using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketsAPI.Models;
using MovieTicketsAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentsController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        // GET: api/Payments
        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _paymentRepository.GetAllPayments();
            return Ok(payments);
        }

        // GET: api/Payments/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(string id)
        {
            var payment = await _paymentRepository.GetPaymentById(id);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        // GET: api/Payments/booking/{bookingId}
        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> GetPaymentsByBookingId(string bookingId)
        {
            var payments = await _paymentRepository.GetPaymentsByBookingId(bookingId);
            return Ok(payments);
        }

        // POST: api/Payments
        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] Payment payment)
        {
            if (payment == null || string.IsNullOrEmpty(payment.BookingId) || payment.Amount <= 0 ||
                string.IsNullOrEmpty(payment.PaymentMethod))
            {
                return BadRequest("Invalid payment data.");
            }

            payment.PaymentDate = DateTime.UtcNow;

            try
            {
                await _paymentRepository.CreatePayment(payment);
                return StatusCode(StatusCodes.Status201Created, payment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating payment: {ex.Message}");
            }
        }

        // PUT: api/Payments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(string id, [FromBody] Payment payment)
        {
            if (payment == null || payment.Id != id)
            {
                return BadRequest("Invalid payment data.");
            }

            var existingPayment = await _paymentRepository.GetPaymentById(id);
            if (existingPayment == null)
            {
                return NotFound();
            }

            try
            {
                await _paymentRepository.UpdatePayment(id, payment);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating payment: {ex.Message}");
            }
        }

        // DELETE: api/Payments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(string id)
        {
            var existingPayment = await _paymentRepository.GetPaymentById(id);
            if (existingPayment == null)
            {
                return NotFound();
            }

            try
            {
                await _paymentRepository.DeletePayment(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting payment: {ex.Message}");
            }
        }
    }
}
