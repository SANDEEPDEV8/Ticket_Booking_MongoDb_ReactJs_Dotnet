using MovieTicketsAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository
{
    public interface ITheatreRepository
    {
        Task CreateTheatreAsync(Theatre theatre);
        Task DeleteTheatreAsync(string id);
        Task<List<Theatre>> GetAllTheatresAsync();
        Task<Theatre> GetTheatreByIdAsync(string id);
        Task UpdateTheatreAsync(string id, Theatre updatedTheatre);
    }
}