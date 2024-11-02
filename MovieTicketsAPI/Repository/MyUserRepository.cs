using MongoDB.Driver;
using MovieTicketsAPI.Data;
using MovieTicketsAPI.Models;
using MovieTicketsAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository
{
    public class MyUserRepository : IMyUserRepository
    {
        private readonly MongoDbContext _context;

        public MyUserRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MyUser>> GetAllUsers()
        {
            return await _context.Users.Find(user => true).ToListAsync();
        }

        public async Task<MyUser> GetUserById(string id)
        {
            return await _context.Users.Find(user => user.Id == id).FirstOrDefaultAsync();
        }

        public async Task<MyUser> GetUserByEmail(string email)
        {
            return await _context.Users.Find(user => user.Email == email).FirstOrDefaultAsync();
        }

        public async Task CreateUser(MyUser user)
        {
            await _context.Users.InsertOneAsync(user);
        }

        public async Task UpdateUser(string id, MyUser user)
        {
            await _context.Users.ReplaceOneAsync(u => u.Id == id, user);
        }

        public async Task DeleteUser(string id)
        {
            await _context.Users.DeleteOneAsync(user => user.Id == id);
        }
    }
}
