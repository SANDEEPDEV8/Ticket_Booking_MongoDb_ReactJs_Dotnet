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
    public class TimeslotsController : ControllerBase
    {
        private readonly ITimeslotRepository _timeslotRepository;

        public TimeslotsController(ITimeslotRepository timeslotRepository)
        {
            _timeslotRepository = timeslotRepository;
        }

        // GET: api/Timeslots
        [HttpGet]
        public async Task<IActionResult> GetAllTimeslots()
        {
            var timeslots = await _timeslotRepository.GetAllTimeslots();
            return Ok(timeslots);
        }

        // GET: api/Timeslots/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTimeslotById(string id)
        {
            var timeslot = await _timeslotRepository.GetTimeslotById(id);
            if (timeslot == null)
            {
                return NotFound();
            }
            return Ok(timeslot);
        }

        // POST: api/Timeslots
        [HttpPost]
        public async Task<IActionResult> CreateTimeslot([FromBody] Timeslot timeslot)
        {
            if (timeslot == null )
            {
                return BadRequest("Invalid timeslot data.");
            }

            try
            {
                await _timeslotRepository.CreateTimeslot(timeslot);
                return StatusCode(StatusCodes.Status201Created, timeslot);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating timeslot: {ex.Message}");
            }
        }

        // PUT: api/Timeslots/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTimeslot(string id, [FromBody] Timeslot timeslot)
        {
            if (timeslot == null || timeslot.Id != id)
            {
                return BadRequest("Invalid timeslot data.");
            }

            var existingTimeslot = await _timeslotRepository.GetTimeslotById(id);
            if (existingTimeslot == null)
            {
                return NotFound();
            }

            try
            {
                await _timeslotRepository.UpdateTimeslot(id, timeslot);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating timeslot: {ex.Message}");
            }
        }

        // DELETE: api/Timeslots/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimeslot(string id)
        {
            var existingTimeslot = await _timeslotRepository.GetTimeslotById(id);
            if (existingTimeslot == null)
            {
                return NotFound();
            }

            try
            {
                await _timeslotRepository.DeleteTimeslot(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting timeslot: {ex.Message}");
            }
        }
    }
}
