using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Entities;

namespace FantasyFights.DAL.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user);

        Task<List<User>> GetAllUsers();

        Task<User?> GetUserByEmail(string email);

        Task<User?> GetUserById(int id);
    }
}