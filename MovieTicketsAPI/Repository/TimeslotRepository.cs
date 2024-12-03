using MongoDB.Driver;
using MovieTicketsAPI.Data;
using MovieTicketsAPI.Models;
using MovieTicketsAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository
{
    public class TimeslotRepository : ITimeslotRepository
    {
        private readonly MongoDbContext _context;

        public TimeslotRepository(MongoDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<IEnumerable<Timeslot>> GetAllTimeslots()
        {
            return await _context.Timeslots.Find(timeslot => true).ToListAsync();
        }

        public async Task<Timeslot> GetTimeslotById(string id)
        {
            return await _context.Timeslots.Find(timeslot => timeslot.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateTimeslot(Timeslot timeslot)
        {
            await _context.Timeslots.InsertOneAsync(timeslot);
        }

        public async Task UpdateTimeslot(string id, Timeslot timeslot)
        {
            await _context.Timeslots.ReplaceOneAsync(t => t.Id == id, timeslot);
        }

        public async Task DeleteTimeslot(string id)
        {
            await _context.Timeslots.DeleteOneAsync(timeslot => timeslot.Id == id);
        }
    }
}
