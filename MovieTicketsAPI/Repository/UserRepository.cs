using MongoDB.Driver;
using MovieTicketsAPI.Data;
using MovieTicketsAPI.Models;
using MovieTicketsAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MongoDbContext _context;

        public UserRepository(MongoDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var customers = await _context.Customers.Find(user => true).ToListAsync();
            var admins = await _context.Admins.Find(user => true).ToListAsync();

            customers.ForEach(user => user.Role = "User");
            admins.ForEach(user => user.Role = "Admin");

            return customers.Concat(admins);
        }

        public async Task<User> GetUserById(string id)
        {
            var adminUser = await _context.Admins.Find(user => user.Id == id).FirstOrDefaultAsync();
            if (adminUser != null)
            {
                adminUser.Role = "Admin";
                return adminUser;
            }

            var customerUser = await _context.Customers.Find(user => user.Id == id).FirstOrDefaultAsync();
            if (customerUser != null)
            {
                customerUser.Role = "User";
            }

            return customerUser;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var adminUser = await _context.Admins.Find(user => user.Email == email).FirstOrDefaultAsync();
            if (adminUser != null)
            {
                adminUser.Role = "Admin";
                return adminUser;
            }

            var customerUser = await _context.Customers.Find(user => user.Email == email).FirstOrDefaultAsync();
            if (customerUser != null)
            {
                customerUser.Role = "User";
            }

            return customerUser;
        }


        public async Task CreateUser(User user)
        {
            var existingUser = await GetUserByEmail(user.Email);
            if (existingUser != null)
            {
                throw new Exception("User with the same email already exists.");
            }

            // Set the role and insert based on the intended collection
            if (user.Role == "Admin")
            {
                user.Role = "Admin"; // Explicitly set role to "Admin"
                await _context.Admins.InsertOneAsync(user);
            }
            else
            {
                user.Role = "User"; // Default to "User"
                await _context.Customers.InsertOneAsync(user);
            }
        }

        public async Task UpdateUser(string id, User user)
        {
            // Update in the correct collection based on where the user exists
            var adminUser = await _context.Admins.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (adminUser != null)
            {
                user.Role = "Admin"; // Ensure role is consistent
                await _context.Admins.ReplaceOneAsync(u => u.Id == id, user);
            }
            else
            {
                user.Role = "User"; // Ensure role is consistent
                await _context.Customers.ReplaceOneAsync(u => u.Id == id, user);
            }
        }

        public async Task DeleteUser(string id)
        {
            // Delete from the correct collection based on where the user exists
            var adminUser = await _context.Admins.Find(user => user.Id == id).FirstOrDefaultAsync();
            if (adminUser != null)
            {
                await _context.Admins.DeleteOneAsync(user => user.Id == id);
            }
            else
            {
                await _context.Customers.DeleteOneAsync(user => user.Id == id);
            }
        }

        //public async Task<IEnumerable<User>> GetAllUsers()
        //{
        //    return await _context.Users.Find(user => true).ToListAsync();
        //}

        //public async Task<User> GetUserById(string id)
        //{
        //    return await _context.Users.Find(user => user.Id == id).FirstOrDefaultAsync();
        //}

        //public async Task<User> GetUserByEmail(string email)
        //{
        //    return await _context.Users.Find(user => user.Email == email).FirstOrDefaultAsync();
        //}

        //public async Task CreateUser(User user)
        //{
        //    await _context.Users.InsertOneAsync(user);
        //}

        //public async Task UpdateUser(string id, User user)
        //{
        //    await _context.Users.ReplaceOneAsync(u => u.Id == id, user);
        //}

        //public async Task DeleteUser(string id)
        //{
        //    await _context.Users.DeleteOneAsync(user => user.Id == id);
        //}
    }
}
