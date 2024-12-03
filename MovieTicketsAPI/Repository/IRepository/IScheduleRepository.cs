using Microsoft.EntityFrameworkCore;
using MovieTicketsAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository.IRepository
{
    public interface IScheduleRepository
    {
        Task<IEnumerable<Schedule>> GetAllSchedules();


        Task<IEnumerable<Schedule>> GetPaginatedSchedules(int pageNumber, int pageSize);

  Task<long> GetTotalSchedulesCount();

        Task<Schedule> GetScheduleById(string id);
        Task<IEnumerable<Schedule>> GetSchedulesByMovieId(string movieId);
        Task<IEnumerable<Schedule>> GetSchedulesByTheatreAndTimeslot(string theatreId, string timeSlotId);
        Task CreateSchedule(Schedule schedule);
        Task UpdateSchedule(string id, Schedule schedule);
        Task DeleteSchedule(string id);
    }
}
