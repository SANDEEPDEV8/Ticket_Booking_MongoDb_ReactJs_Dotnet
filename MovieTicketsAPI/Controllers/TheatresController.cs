using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieTicketsAPI.Repository;
using MovieTicketsAPI.Models;

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

        // GET: api/<TheatreController>
        [HttpGet]
        public async Task<IEnumerable<Theatre>> Get()
        {
            return await _theatreRepository.GetAllTheatresAsync();
        }

        // GET api/<TheatreController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Theatre>> Get(string id)
        {
            var theatre = await _theatreRepository.GetTheatreByIdAsync(id);
            if (theatre == null)
            {
                return NotFound();
            }
            return theatre;
        }

        // POST api/<TheatreController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Theatre theatre)
        {
            await _theatreRepository.CreateTheatreAsync(theatre);
            return CreatedAtAction(nameof(Get), new { id = theatre.Id }, theatre);
        }

        // PUT api/<TheatreController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Theatre updatedTheatre)
        {
            var theatre = await _theatreRepository.GetTheatreByIdAsync(id);
            if (theatre == null)
            {
                return NotFound();
            }
            await _theatreRepository.UpdateTheatreAsync(id, updatedTheatre);
            return NoContent();
        }

        // DELETE api/<TheatreController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var theatre = await _theatreRepository.GetTheatreByIdAsync(id);
            if (theatre == null)
            {
                return NotFound();
            }
            await _theatreRepository.DeleteTheatreAsync(id);
            return NoContent();
        }
    }
}