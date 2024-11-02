using MongoDB.Driver;
using MovieTicketsAPI.Data;
using MovieTicketsAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository
{
    public class TheatreRepository : ITheatreRepository
    {
        private MongoDbContext _context;
        public TheatreRepository(MongoDbContext dbContext)
        {
            _context = dbContext;
        }

        // Create
        public async Task CreateTheatreAsync(Theatre theatre)
        {
            await _context.Theatres.InsertOneAsync(theatre);
        }

        // Read
        public async Task<Theatre> GetTheatreByIdAsync(string id)
        {
            return await _context.Theatres.Find(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Theatre>> GetAllTheatresAsync()
        {
            return await _context.Theatres.Find(t => true).ToListAsync();
        }

        // Update
        public async Task UpdateTheatreAsync(string id, Theatre updatedTheatre)
        {
            await _context.Theatres.ReplaceOneAsync(t => t.Id == id, updatedTheatre);
        }

        // Delete
        public async Task DeleteTheatreAsync(string id)
        {
            await _context.Theatres.DeleteOneAsync(t => t.Id == id);
        }
    }
}