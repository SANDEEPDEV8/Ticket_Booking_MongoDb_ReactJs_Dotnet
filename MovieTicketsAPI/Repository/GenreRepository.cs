using MongoDB.Driver;
using MovieTicketsAPI.Data;
using MovieTicketsAPI.Models;
using MovieTicketsAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly MongoDbContext _context;

        public GenreRepository(MongoDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<IEnumerable<Genre>> GetAllGenres()
        {
            return await _context.Genres.Find(genre => true).ToListAsync();
        }

        public async Task<Genre> GetGenreById(string id)
        {
            return await _context.Genres.Find(genre => genre.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Genre> GetGenreByName(string name)
        {
            return await _context.Genres.Find(genre => genre.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }

        public async Task CreateGenre(Genre genre)
        {
            await _context.Genres.InsertOneAsync(genre);
        }

        public async Task UpdateGenre(string id, Genre genre)
        {
            await _context.Genres.ReplaceOneAsync(g => g.Id == id, genre);
        }

        public async Task DeleteGenre(string id)
        {
            await _context.Genres.DeleteOneAsync(genre => genre.Id == id);
        }
    }
}
