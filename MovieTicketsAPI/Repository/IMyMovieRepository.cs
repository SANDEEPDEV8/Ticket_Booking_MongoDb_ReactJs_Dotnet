using MovieTicketsAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository
{
    public interface IMyMovieRepository
    {
        Task CreateMovie(MyMovie movie);
        Task DeleteMovie(string id);
        Task<IEnumerable<MyMovie>> GetAllMovies();
        Task<IEnumerable<MyMovie>> GetAllPaginatedMovies(int pageNumber, int pageSize, string sort, string movieName,string theatreId);
        Task<long> GetCount();
        Task<List<MyMovie>> GetMoviesByName(string movieName);
        Task<MyMovie> GetMovie(string id);
        Task UpdateMovie(string id, MyMovie movie);
    }
}