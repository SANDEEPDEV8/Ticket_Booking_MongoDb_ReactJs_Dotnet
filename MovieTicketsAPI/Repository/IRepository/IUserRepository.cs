using MovieTicketsAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        Task CreateUser(User user);
        Task DeleteUser(string id);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(string id);
        Task<User> GetUserByEmail(string email);
        Task UpdateUser(string id, User user);
    }
}