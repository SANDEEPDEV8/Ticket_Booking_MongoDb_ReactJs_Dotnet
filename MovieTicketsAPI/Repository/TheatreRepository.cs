using MongoDB.Driver;
using MovieTicketsAPI.Data;
using MovieTicketsAPI.Models;
using MovieTicketsAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository
{
    public class TheatreRepository : ITheatreRepository
    {
        private readonly MongoDbContext _context;

        public TheatreRepository(MongoDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<IEnumerable<Theatre>> GetAllTheatres()
        {
            return await _context.Theatres.Find(theatre => true).ToListAsync();
        }

        public async Task<Theatre> GetTheatreById(string id)
        {
            return await _context.Theatres.Find(theatre => theatre.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateTheatre(Theatre theatre)
        {
            await _context.Theatres.InsertOneAsync(theatre);
        }

        public async Task UpdateTheatre(string id, Theatre theatre)
        {
            await _context.Theatres.ReplaceOneAsync(t => t.Id == id, theatre);
        }

        public async Task DeleteTheatre(string id)
        {
            await _context.Theatres.DeleteOneAsync(theatre => theatre.Id == id);
        }
    }
}
