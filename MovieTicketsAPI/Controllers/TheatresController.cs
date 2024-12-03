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
    public class TheatresController : ControllerBase
    {
        private readonly ITheatreRepository _theatreRepository;

        public TheatresController(ITheatreRepository theatreRepository)
        {
            _theatreRepository = theatreRepository;
        }

        // GET: api/Theatres
        [HttpGet]
        public async Task<IActionResult> GetAllTheatres()
        {
            var theatres = await _theatreRepository.GetAllTheatres();
            return Ok(theatres);
        }

        // GET: api/Theatres/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTheatreById(string id)
        {
            var theatre = await _theatreRepository.GetTheatreById(id);
            if (theatre == null)
            {
                return NotFound();
            }
            return Ok(theatre);
        }

        // POST: api/Theatres
        [HttpPost]
        public async Task<IActionResult> CreateTheatre([FromBody] Theatre theatre)
        {
            if (theatre == null || theatre.ScreenNumber <= 0 || theatre.SeatCapacity <= 0 || string.IsNullOrEmpty(theatre.Location))
            {
                return BadRequest("Invalid theatre data.");
            }

            try
            {
                await _theatreRepository.CreateTheatre(theatre);
                return StatusCode(StatusCodes.Status201Created, theatre);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating theatre: {ex.Message}");
            }
        }

        // PUT: api/Theatres/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTheatre(string id, [FromBody] Theatre theatre)
        {
            if (theatre == null || theatre.Id != id)
            {
                return BadRequest("Invalid theatre data.");
            }

            var existingTheatre = await _theatreRepository.GetTheatreById(id);
            if (existingTheatre == null)
            {
                return NotFound();
            }

            try
            {
                await _theatreRepository.UpdateTheatre(id, theatre);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating theatre: {ex.Message}");
            }
        }

        // DELETE: api/Theatres/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTheatre(string id)
        {
            var existingTheatre = await _theatreRepository.GetTheatreById(id);
            if (existingTheatre == null)
            {
                return NotFound();
            }

            try
            {
                await _theatreRepository.DeleteTheatre(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting theatre: {ex.Message}");
            }
        }
    }
}
