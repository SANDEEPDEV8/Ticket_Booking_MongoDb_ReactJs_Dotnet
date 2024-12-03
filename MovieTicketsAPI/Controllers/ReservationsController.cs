using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketsAPI.Data;
using MovieTicketsAPI.Models;
using MovieTicketsAPI.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationRepository _myReservationRepository;

        public ReservationsController( IReservationRepository myReservationRepository)
        {
            _myReservationRepository = myReservationRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Reservation reservationObj)
        {

            reservationObj.ReservationTime = DateTime.Now;

            var result = await _myReservationRepository.CreateReservationAsync(reservationObj);

            if (result == false)
            {
                return BadRequest();
            }

            //_dbContext.Reservations.Add(reservationObj);
            //_dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        //[Authorize(Roles = "Admin")]
        [Authorize()]
        [HttpGet]
        public async Task<IActionResult> GetReservations(string sort, string keyword, int? pageNumber, int? pageSize)
        {
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 100;
            var customerName = keyword ?? "";

            var reservationsRes = await _myReservationRepository.GetReservationsAsync(customerName, sort, currentPageNumber, currentPageSize);
            var totalCount = await _myReservationRepository.GetTotalCountAsync(customerName);
            var totalPages = (int)Math.Ceiling(totalCount / (double)currentPageSize);

            // create a response variable and assign the reservationsRes to it. all properties are same but set the TotalPages to totalPages
            var response = reservationsRes.Select(reservation => new
            {
                Id = reservation.Id,
                ReservationTime = reservation.ReservationTime,
                CustomerName = reservation.CustomerName,
                MovieName = reservation.MovieName,
                MovieImageUrl = reservation.MovieImageUrl,
                TotalPages = totalPages
            });

            return Ok(response);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationDetail(string id)
        {
            var reservationResult =await _myReservationRepository.GetReservationDetailAsync(id);
            return Ok(reservationResult);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var reservation = await _myReservationRepository.GetReservationById(id);
            if (reservation == null)
            {
                return NotFound("No data found with this Id");
            }
            else
            {
                await _myReservationRepository.DeleteReservationAsync(id);
                return Ok("Successfully deleted data");
            }
        }



        /*
         [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetReservations(string sort, string keyword, int? pageNumber, int? pageSize)
        {
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 100;
            var customerName = keyword ?? "";

            var filteredResult = from reservation in _dbContext.Reservations
                               join customer in _dbContext.Users on reservation.UserId equals customer.Id
                               join movie in _dbContext.Movies on reservation.MovieId equals movie.Id
                               where customer.Name.Contains(customerName)
                               select new
                               {
                                   Id = reservation.Id,
                               };

            var TotalCount = filteredResult.Count();
            var TotalPages = (int)Math.Ceiling(TotalCount / (double)currentPageSize);
            var reservations = from reservation in _dbContext.Reservations
                               join customer in _dbContext.Users on reservation.UserId equals customer.Id
                               join movie in _dbContext.Movies on reservation.MovieId equals movie.Id
                               where customer.Name.Contains(customerName)
                               select new
                               {
                                   Id = reservation.Id,
                                   ReservationTime = reservation.ReservationTime,
                                   CustomerName = customer.Name,
                                   MovieName = movie.Name,
                                   MovieImageUrl = movie.ImageUrl,
                                   TotalPages = TotalPages
                               };

            switch (sort)
            {
                case "movie_name":
                    reservations = reservations.OrderBy(r => r.MovieName);
                    break;
                case "customer_name":
                    reservations = reservations.OrderBy(r => r.CustomerName);
                    break;
                case "created_oldest":
                    reservations = reservations.OrderBy(r => r.Id);
                    break;
                default:
                    reservations = reservations.OrderByDescending(r => r.Id);
                    break;
            }

            return Ok(reservations.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }
         
         */
    }
}
