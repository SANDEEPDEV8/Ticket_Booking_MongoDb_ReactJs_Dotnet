using MongoDB.Driver;
using MovieTicketsAPI.Data;
using MovieTicketsAPI.Models;
using MovieTicketsAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly MongoDbContext _context;

        public ScheduleRepository(MongoDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<IEnumerable<Schedule>> GetAllSchedules()
        {
            return await _context.Schedules.Find(schedule => true).ToListAsync();
        }

        public async Task<IEnumerable<Schedule>> GetPaginatedSchedules(int pageNumber, int pageSize)
        {
            return await _context.Schedules
                                 .Find(schedule => true)
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Limit(pageSize)
                                 .ToListAsync();
        }

        public async Task<long> GetTotalSchedulesCount()
        {
            return await _context.Schedules.CountDocumentsAsync(schedule => true);
        }


        public async Task<Schedule> GetScheduleById(string id)
        {
            return await _context.Schedules.Find(schedule => schedule.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Schedule>> GetSchedulesByMovieId(string movieId)
        {
            return await _context.Schedules.Find(schedule => schedule.MovieId == movieId).ToListAsync();
        }

        public async Task<IEnumerable<Schedule>> GetSchedulesByTheatreAndTimeslot(string theatreId, string timeSlotId)
        {
            var filter = Builders<Schedule>.Filter.Eq(s => s.TheatreId, theatreId) &
                         Builders<Schedule>.Filter.Eq(s => s.TimeSlotId, timeSlotId);
            return await _context.Schedules.Find(filter).ToListAsync();
        }

        public async Task CreateSchedule(Schedule schedule)
        {
            await _context.Schedules.InsertOneAsync(schedule);
        }

        public async Task UpdateSchedule(string id, Schedule schedule)
        {
            await _context.Schedules.ReplaceOneAsync(s => s.Id == id, schedule);
        }

        public async Task DeleteSchedule(string id)
        {
            await _context.Schedules.DeleteOneAsync(schedule => schedule.Id == id);
        }
    }
}
