using MovieTicketsAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository.IRepository
{
    public interface ITimeslotRepository
    {
        Task<IEnumerable<Timeslot>> GetAllTimeslots();
        Task<Timeslot> GetTimeslotById(string id);
        Task CreateTimeslot(Timeslot timeslot);
        Task UpdateTimeslot(string id, Timeslot timeslot);
        Task DeleteTimeslot(string id);
    }
}
