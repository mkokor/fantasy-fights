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
    }
}