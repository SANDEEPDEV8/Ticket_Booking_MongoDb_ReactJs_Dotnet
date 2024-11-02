using MovieTicketsAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository.IRepository
{
    public interface IMyUserRepository
    {
        Task CreateUser(MyUser user);
        Task DeleteUser(string id);
        Task<IEnumerable<MyUser>> GetAllUsers();
        Task<MyUser> GetUserById(string id);
        Task<MyUser> GetUserByEmail(string email);
        Task UpdateUser(string id, MyUser user);
    }
}