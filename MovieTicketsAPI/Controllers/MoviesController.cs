using DnsClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MovieTicketsAPI.Data;
using MovieTicketsAPI.Models;
using MovieTicketsAPI.Repository;
using MovieTicketsAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private MovieDbContext _dbContext;
        private readonly IMyMovieRepository _myMovieRepository;

        public MoviesController(MovieDbContext dbContext, IMyMovieRepository myMovieRepository)
        {
            _dbContext = dbContext;
            _myMovieRepository = myMovieRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovies(string sort, string keyword, int? pageNumber, int? pageSize, string theatreId)
        {
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 100;
            var movieName = keyword ?? "";


            var TotalCount = await _myMovieRepository.GetCount();// _dbContext.Movies.Where(q => q.Name.Contains(movieName)).Count();
            var TotalPages = Math.Ceiling(((decimal)TotalCount / (decimal)currentPageSize));
            var movies = await _myMovieRepository.GetAllPaginatedMovies(currentPageNumber, currentPageSize, sort, keyword, theatreId);
            var moviesData = movies.Select(movie =>
                new
                {
                    Id = movie.Id,
                    Name = movie.Name,
                    Duration = movie.Duration,
                    Language = movie.Language,
                    Rating = movie.Rating,
                    Genre = movie.Genre,
                    ImageUrl = movie.ImageUrl,
                    TotalPages = TotalPages,
                });


            return Ok(moviesData);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovie(string id)
        {
            var movie = await _myMovieRepository.GetMovie(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Search(string movieName)
        {
            var movies = await _myMovieRepository.GetMoviesByName(movieName);
            var result = movies.Select(m => new
            {
                Id = m.Id,
                Name = m.Name,
                Duration = m.Duration,
                Language = m.Language,
                Rating = m.Rating,
                Genre = m.Genre,
                ImageUrl = m.ImageUrl
            });
            return Ok(movies);  
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] MyMovie movieObj)
        {
            var guid = Guid.NewGuid();
            var filePath = Path.Combine("wwwroot", guid + ".jpg");
            if (movieObj.Image != null)
            {
                var fileStream = new FileStream(filePath, FileMode.Create);
                movieObj.Image.CopyTo(fileStream);
            }


            movieObj.ImageUrl = filePath.Remove(0, 7);
            await _myMovieRepository.CreateMovie(movieObj);
            return StatusCode(StatusCodes.Status201Created);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromForm] MyMovie movieObj)
        {
            //var movie = _dbContext.Movies.Find(id);
            var movie = await _myMovieRepository.GetMovie(id);
            if (movie == null)
            {
                return NotFound("No data found with this Id");
            }
            else
            {
                var guid = Guid.NewGuid();
                var filePath = Path.Combine("wwwroot", guid + ".jpg");
                if (movieObj.Image != null)
                {
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    movieObj.Image.CopyTo(fileStream);
                    movie.ImageUrl = filePath.Remove(0, 7);
                }

                movie.Name = movieObj.Name;
                movie.Description = movieObj.Description;
                movie.TheatreId = movieObj.TheatreId;
                movie.Language = movieObj.Language;
                movie.Duration = movieObj.Duration;
                movie.PlayingDate = movieObj.PlayingDate;
                movie.PlayingTime = movie.PlayingTime;
                movie.Rating = movieObj.Rating;
                movie.Genre = movieObj.Genre;
                movie.TrailorUrl = movieObj.TrailorUrl;
                movie.TicketPrice = movieObj.TicketPrice;
                await _myMovieRepository.UpdateMovie(id, movie);
                return Ok("Successfully updated data");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var movie = await _myMovieRepository.GetMovie(id);
            if (movie == null)
            {
                return NotFound("No data found with this Id");
            }
            else
            {
                await _myMovieRepository.DeleteMovie(id);
                return Ok("Successfully deleted data");
            }
        }
    }
}
