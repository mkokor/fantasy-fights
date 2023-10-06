using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Entities;

namespace FantasyFights.DAL.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _databaseContext;

        public UserRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<User> CreateUser(User user)
        {
            await _databaseContext.Users.AddAsync(user);
            return user;
        }
    }
}