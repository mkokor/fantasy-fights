using FantasyFights.DAL.Entities;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<User>> GetAllUsers()
        {
            return await _databaseContext.Users.ToListAsync();
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _databaseContext.Users.FirstOrDefaultAsync(user => user.Email.Equals(email));
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _databaseContext.Users.FindAsync(id);
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _databaseContext.Users.FirstOrDefaultAsync(user => user.Username.Equals(username));
        }
    }
}