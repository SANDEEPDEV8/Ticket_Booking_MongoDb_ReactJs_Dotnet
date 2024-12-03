using MovieTicketsAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository.IRepository
{
    public interface IMovieRepository
    {
        Task CreateMovie(Movie movie);
        Task DeleteMovie(string id);
        Task<IEnumerable<Movie>> GetAllMovies();
        Task<IEnumerable<Movie>> GetAllPaginatedMovies(int pageNumber, int pageSize, string sort, string movieName, string theatreId);
        Task<long> GetCount();
        Task<List<Movie>> GetMoviesByName(string movieName);
        Task<Movie> GetMovie(string id);
        Task UpdateMovie(string id, Movie movie);
    }
}