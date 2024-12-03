using MovieTicketsAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository.IRepository
{
    public interface ITheatreRepository
    {
        Task<IEnumerable<Theatre>> GetAllTheatres();
        Task<Theatre> GetTheatreById(string id);
        Task CreateTheatre(Theatre theatre);
        Task UpdateTheatre(string id, Theatre theatre);
        Task DeleteTheatre(string id);
    }
}
