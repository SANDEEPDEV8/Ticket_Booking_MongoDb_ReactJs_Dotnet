using MovieTicketsAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository.IRepository
{
    public interface IGenreRepository
    {
        Task CreateGenre(Genre genre);
        Task DeleteGenre(string id);
        Task<IEnumerable<Genre>> GetAllGenres();
        Task<Genre> GetGenreById(string id);
        Task<Genre> GetGenreByName(string name);
        Task UpdateGenre(string id, Genre genre);
    }
}