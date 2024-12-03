using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketsAPI.Models;
using MovieTicketsAPI.Repository;
using MovieTicketsAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _myUserRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ITimeslotRepository _timeslotRepository;
        private readonly IMovieRepository _myMovieRepository;
        private readonly ITheatreRepository _theatreRepository;

        public BookingsController(IBookingRepository bookingRepository,
            IUserRepository myUserRepository,
            IScheduleRepository scheduleRepository,
            ITimeslotRepository timeslotRepository,
            IMovieRepository myMovieRepository,
            ITheatreRepository theatreRepository
            )
        {
            _bookingRepository = bookingRepository;
            _myUserRepository = myUserRepository;
            _scheduleRepository = scheduleRepository;
            _timeslotRepository = timeslotRepository;
            _myMovieRepository = myMovieRepository;
            _theatreRepository = theatreRepository;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _bookingRepository.GetAllBookings();
            return Ok(bookings);
        }

        // GET: api/Bookings/paginated
        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginatedBookings(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }

            try
            {
                // Extract the user's information from the claims
                var user = HttpContext.User;
                var emailClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
                var roleClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

                if (emailClaim == null || roleClaim == null)
                {
                    return Unauthorized("Unable to determine user identity.");
                }

                string customerId = null;
                if (roleClaim == "User")
                {
                    // Get the user by email to retrieve the CustomerId
                    var userEntity = await _myUserRepository.GetUserByEmail(emailClaim);
                    if (userEntity == null)
                    {
                        return Unauthorized("User not found.");
                    }
                    customerId = userEntity.Id;
                }

                // Fetch paginated bookings based on customerId
                var bookings = await _bookingRepository.GetPaginatedBookings(pageNumber, pageSize, customerId);
                long totalBookings = string.IsNullOrEmpty(customerId)
                    ? await _bookingRepository.GetTotalBookingsCount()
                    : bookings.Count();

                var totalPages = (int)Math.Ceiling((double)totalBookings / pageSize);

                // Enrich each booking with data from Schedules, Timeslots, Users, and Movies
                var enrichedBookings = new List<object>();

                foreach (var booking in bookings)
                {
                    // Fetch User (Customer) data
                    var customer = await _myUserRepository.GetUserById(booking.CustomerId);
                    var customerName = customer != null ? customer.Name : "N/A";

                    // Fetch Schedule data
                    var schedule = await _scheduleRepository.GetScheduleById(booking.ScheduleId);
                    if (schedule == null) continue;

                    // Fetch Timeslot data
                    var timeslot = await _timeslotRepository.GetTimeslotById(schedule.TimeSlotId);
                    var scheduleTimeSlot = timeslot != null ? $"{timeslot.StartTime} - {timeslot.EndTime}" : "N/A";

                    // Fetch Movie data
                    var movie = await _myMovieRepository.GetMovie(schedule.MovieId);
                    var movieName = movie != null ? movie.Name : "N/A";
                    var movieImageUrl = movie != null ? movie.ImageUrl : null;

                    // Fetch Theatre data
                    var theatre = await _theatreRepository.GetTheatreById(schedule.TheatreId);
                    var theatreName = theatre != null ? theatre.Name : "N/A";

                    // Construct enriched booking object
                    enrichedBookings.Add(new
                    {
                        Id = booking.Id,
                        ReservationTime = scheduleTimeSlot,
                        CustomerName = customerName,
                        MovieName = movieName,
                        MovieImageUrl = movieImageUrl,
                        location = $"{theatre.Name} screen {theatre.ScreenNumber} at {theatre.Location}",
                        NumberOfSeats = booking.NumberOfSeats,
                        TotalPrice = booking.TotalPrice,
                        BookingDate = booking.BookingDate,
                        SeatNumbers = string.Join(", ", booking.SeatNumbers),
                        TotalPages = totalPages
                    });
                }

                return Ok(enrichedBookings);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving paginated bookings: {ex.Message}");
            }
        }



        // GET: api/Bookings/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(string id)
        {
            var booking = await _bookingRepository.GetBookingById(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        // GET: api/Bookings/customer/{customerId}
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetBookingsByCustomerId(string customerId)
        {
            var bookings = await _bookingRepository.GetBookingsByCustomerId(customerId);
            return Ok(bookings);
        }

        // POST: api/Bookings
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] Booking booking)
        {
            if (booking == null || booking.NumberOfSeats <= 0 || string.IsNullOrEmpty(booking.ScheduleId) || string.IsNullOrEmpty(booking.CustomerId))
            {
                return BadRequest("Invalid booking data.");
            }

            try
            {
                var schedule = await _scheduleRepository.GetScheduleById(booking.ScheduleId);
                if (schedule == null)
                {
                    return NotFound("Schedule not found.");
                }

                var theatre = await _theatreRepository.GetTheatreById(schedule.TheatreId);
                if (theatre == null)
                {
                    return BadRequest("Invalid theatre ID.");
                }

                int seatCapacity = theatre.SeatCapacity;

                // Get existing bookings for this schedule
                var existingBookings = await _bookingRepository.GetBookingsByScheduleId(booking.ScheduleId);
                var bookedSeats = existingBookings.SelectMany(b => b.SeatNumbers).ToList();

                // Allocate seats
                List<int> allocatedSeats = new List<int>();
                int availableSeat = 1;
                for (int i = 0; i < booking.NumberOfSeats; i++)
                {
                    // Find the next available seat
                    while (bookedSeats.Contains(availableSeat) && availableSeat <= seatCapacity)
                    {
                        availableSeat++;
                    }

                    if (availableSeat > seatCapacity)
                    {
                        return BadRequest("Insufficient available seats for the requested number of seats.");
                    }

                    allocatedSeats.Add(availableSeat);
                    bookedSeats.Add(availableSeat);
                }

                // Assign allocated seats and calculate total price
                booking.SeatNumbers = allocatedSeats;
                booking.TotalPrice = booking.NumberOfSeats * schedule.Price;

                await _bookingRepository.CreateBooking(booking);
                return StatusCode(StatusCodes.Status201Created, booking);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating booking: {ex.Message}");
            }
        }


        // PUT: api/Bookings/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(string id, [FromBody] Booking booking)
        {
            if (booking == null || booking.Id != id)
            {
                return BadRequest("Invalid booking data.");
            }

            var existingBooking = await _bookingRepository.GetBookingById(id);
            if (existingBooking == null)
            {
                return NotFound();
            }

            try
            {
                await _bookingRepository.UpdateBooking(id, booking);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating booking: {ex.Message}");
            }
        }

        // DELETE: api/Bookings/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(string id)
        {
            var existingBooking = await _bookingRepository.GetBookingById(id);
            if (existingBooking == null)
            {
                return NotFound();
            }

            try
            {
                await _bookingRepository.DeleteBooking(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting booking: {ex.Message}");
            }
        }
    }
}
