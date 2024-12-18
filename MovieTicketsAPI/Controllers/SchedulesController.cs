using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketsAPI.Models;
using MovieTicketsAPI.Repository;
using MovieTicketsAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly ITheatreRepository _theatreRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ITimeslotRepository _timeslotRepository;
        private readonly IBookingRepository _bookingRepository;

        public SchedulesController(IScheduleRepository scheduleRepository, 
                ITheatreRepository theatreRepository, ITimeslotRepository timeslotRepository,
                IBookingRepository bookingRepository)
        {
            _scheduleRepository = scheduleRepository;
            _theatreRepository = theatreRepository;
            _timeslotRepository = timeslotRepository;
            _bookingRepository = bookingRepository;
        }

        // GET: api/Schedules
        [HttpGet]
        public async Task<IActionResult> GetAllSchedules()
        {
            var schedules = await _scheduleRepository.GetAllSchedules();
            return Ok(schedules);
        }

        // GET: api/Schedules/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetScheduleById(string id)
        {
            var schedule = await _scheduleRepository.GetScheduleById(id);
            if (schedule == null)
            {
                return NotFound();
            }
            return Ok(schedule);
        }

        // GET: api/Schedules/movie/{movieId}
        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetSchedulesByMovieId(string movieId)
        {
            var schedules = await _scheduleRepository.GetSchedulesByMovieId(movieId);
            return Ok(schedules);
        }

        [HttpGet("details/{movieId}")]
        public async Task<IActionResult> GetScheduleDetailsByMovieId(string movieId)
        {
            try
            {
                // Step 1: Get all schedules by movieId
                var schedules = await _scheduleRepository.GetSchedulesByMovieId(movieId);

                if (schedules == null || !schedules.Any())
                {
                    return NotFound("No schedules found for the specified movie ID.");
                }

                // Step 2: Prepare response data with theatre and timeslot information
                var scheduleDetails = new List<object>();

                foreach (var schedule in schedules)
                {
                    // Fetch the corresponding theatre details
                    var theatre = await _theatreRepository.GetTheatreById(schedule.TheatreId);
                    if (theatre == null)
                    {
                        return NotFound($"Theatre with ID {schedule.TheatreId} not found.");
                    }

                    // Fetch the corresponding timeslot details
                    var timeslot = await _timeslotRepository.GetTimeslotById(schedule.TimeSlotId);
                    if (timeslot == null)
                    {
                        return NotFound($"Timeslot with ID {schedule.TimeSlotId} not found.");
                    }

                    var bookings = await _bookingRepository.GetBookingsByScheduleId(schedule.Id);
                    int seatsBooked = bookings.Sum(b => b.SeatNumbers?.Count ?? 0);

                    int totalSeats = theatre.SeatCapacity;
                    int seatsAvailable = totalSeats - seatsBooked;

                    // Step 3: Construct the response object for each schedule
                    var scheduleDetail = new
                    {
                        scheduleId = schedule.Id,
                        date = schedule.Date,
                        price = schedule.Price,
                        startTime = timeslot.StartTime,
                        endTime = timeslot.EndTime,
                        screenNumber = theatre.ScreenNumber,
                        location = theatre.Location,
                        seatsBooked = seatsBooked,
                        seatsAvailable = seatsAvailable > 0 ? seatsAvailable : 0
                    };

                    scheduleDetails.Add(scheduleDetail);
                }

                return Ok(scheduleDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving schedules: {ex.Message}");
            }
        }


        // POST: api/Schedules
        // POST: api/Schedules
        [HttpPost]
        public async Task<IActionResult> CreateSchedule([FromBody] Schedule schedule)
        {
            if (schedule == null || string.IsNullOrEmpty(schedule.Date) ||
                schedule.Price <= 0 || string.IsNullOrEmpty(schedule.MovieId) ||
                string.IsNullOrEmpty(schedule.TheatreId) || string.IsNullOrEmpty(schedule.TimeSlotId))
            {
                return BadRequest("Invalid schedule data.");
            }

            try
            {
                //var theatre = await _theatreRepository.GetTheatreById(schedule.TheatreId);
                //if (theatre == null)
                //{
                //    return BadRequest("Invalid theatre ID.");
                //}

                //await _scheduleRepository.CreateSchedule(schedule);
                //return StatusCode(StatusCodes.Status201Created, schedule);

                var theatre = await _theatreRepository.GetTheatreById(schedule.TheatreId);
                if (theatre == null)
                {
                    return BadRequest("Invalid theatre ID.");
                }

                // Check if a schedule with the same theatre, time slot, and date already exists
                var existingSchedules = await _scheduleRepository.GetSchedulesByTheatreAndTimeslot(schedule.TheatreId, schedule.TimeSlotId);
                if (existingSchedules.Any(s => s.Date == schedule.Date))
                {
                    return Conflict("A schedule already exists for the specified theatre, time slot, and date.");
                }

                // Create the schedule if no conflicts are found
                await _scheduleRepository.CreateSchedule(schedule);
                return StatusCode(StatusCodes.Status201Created, schedule);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating schedule: {ex.Message}");
            }
        }


        // PUT: api/Schedules/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule(string id, [FromBody] Schedule schedule)
        {
            if (schedule == null || schedule.Id != id)
            {
                return BadRequest("Invalid schedule data.");
            }

            var existingSchedule = await _scheduleRepository.GetScheduleById(id);
            if (existingSchedule == null)
            {
                return NotFound();
            }

            try
            {
                await _scheduleRepository.UpdateSchedule(id, schedule);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating schedule: {ex.Message}");
            }
        }

        // DELETE: api/Schedules/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(string id)
        {
            var existingSchedule = await _scheduleRepository.GetScheduleById(id);
            if (existingSchedule == null)
            {
                return NotFound();
            }

            try
            {
                await _scheduleRepository.DeleteSchedule(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting schedule: {ex.Message}");
            }
        }
    }
}
