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
    public class GenresController : ControllerBase
    {
        private readonly IGenreRepository _genreRepository;

        public GenresController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        // GET: api/Genres
        [HttpGet]
        public async Task<IActionResult> GetAllGenres()
        {
            var genres = await _genreRepository.GetAllGenres();
            return Ok(genres);
        }

        // GET: api/Genres/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenreById(string id)
        {
            var genre = await _genreRepository.GetGenreById(id);
            if (genre == null)
            {
                return NotFound();
            }
            return Ok(genre);
        }

        // GET: api/Genres/name/{name}
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetGenreByName(string name)
        {
            var genre = await _genreRepository.GetGenreByName(name);
            if (genre == null)
            {
                return NotFound();
            }
            return Ok(genre);
        }

        // POST: api/Genres
        [HttpPost]
        public async Task<IActionResult> CreateGenre([FromBody] Genre genre)
        {
            if (genre == null || string.IsNullOrEmpty(genre.Name))
            {
                return BadRequest("Genre name is required.");
            }

            try
            {
                await _genreRepository.CreateGenre(genre);
                return StatusCode(StatusCodes.Status201Created, genre);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating genre: {ex.Message}");
            }
        }

        // PUT: api/Genres/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(string id, [FromBody] Genre genre)
        {
            if (genre == null || genre.Id != id)
            {
                return BadRequest("Invalid genre data.");
            }

            var existingGenre = await _genreRepository.GetGenreById(id);
            if (existingGenre == null)
            {
                return NotFound();
            }

            try
            {
                await _genreRepository.UpdateGenre(id, genre);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating genre: {ex.Message}");
            }
        }

        // DELETE: api/Genres/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(string id)
        {
            var existingGenre = await _genreRepository.GetGenreById(id);
            if (existingGenre == null)
            {
                return NotFound();
            }

            try
            {
                await _genreRepository.DeleteGenre(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting genre: {ex.Message}");
            }
        }
    }
}
