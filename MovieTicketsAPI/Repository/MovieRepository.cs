using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MovieTicketsAPI.Data;
using MovieTicketsAPI.Models;
using MovieTicketsAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private MongoDbContext _context;
        public MovieRepository(MongoDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<IEnumerable<Movie>> GetAllMovies()
        {
            return await _context.Movies.Find(movie => true).ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetAllPaginatedMovies(int pageNumber, int pageSize, string sort, string movieName, string theatreId)
        {
            var sortDefinitionBuilder = Builders<Movie>.Sort;
            SortDefinition<Movie> sortDefinition;
            switch (sort)
            {
                case "highest_rating":
                    sortDefinition = sortDefinitionBuilder.Descending(m => m.Rating);
                    break;
                case "lowest_rating":
                    sortDefinition = sortDefinitionBuilder.Ascending(m => m.Rating);
                    break;
                case "longest_duration":
                    sortDefinition = sortDefinitionBuilder.Descending(m => m.Duration);
                    break;
                case "shortest_duration":
                    sortDefinition = sortDefinitionBuilder.Ascending(m => m.Duration);
                    break;
                case "created_oldest":
                    sortDefinition = sortDefinitionBuilder.Ascending(m => m.Id);
                    break;
                default:
                    sortDefinition = sortDefinitionBuilder.Descending(m => m.Id);
                    break;
            }

            var filter = Builders<Movie>.Filter.Regex("Name", new MongoDB.Bson.BsonRegularExpression($"^{movieName}", "i"));
            // if theatreId is not null, filter by theatreId
            //if (!string.IsNullOrEmpty(theatreId))
            //{
            //    filter = filter & Builders<Movie>.Filter.Eq("TheatreId", theatreId);
            //}

            return await _context.Movies.Find(movie => filter.Inject())
                                        .Sort(sortDefinition)
                                        .Skip((pageNumber-1)*pageSize)
                                        .Limit(pageSize)
                                        .ToListAsync();
        }

        public async Task<long> GetCount()
        {
            return await _context.Movies.CountDocumentsAsync(movie => true);
        }

        public async Task<Movie> GetMovie(string id)
        {
            return await _context.Movies.Find(movie => movie.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Movie>> GetMoviesByName(string movieName)
        {
            var filter = Builders<Movie>.Filter.Regex("Name", new MongoDB.Bson.BsonRegularExpression($"^{movieName}", "i"));

            // Fetch the movies matching the filter
            var movies = await _context.Movies.Find(x=>filter.Inject())
            
                .ToListAsync();

            return movies;
        }

        public async Task CreateMovie(Movie movie)
        {
            await _context.Movies.InsertOneAsync(movie);
        }

        public async Task UpdateMovie(string id, Movie movie)
        {
            await _context.Movies.ReplaceOneAsync(m => m.Id == id, movie);
        }

        public async Task DeleteMovie(string id)
        {
            await _context.Movies.DeleteOneAsync(movie => movie.Id == id);
        }
    }
}
