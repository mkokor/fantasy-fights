using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Repositories.CharacterRepository;
using FantasyFights.DAL.Repositories.UserRepository;

namespace FantasyFights.DAL.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _databaseContext;

        public ICharacterRepository CharacterRepository { get; }
        public IUserRepository UserRepository { get; }

        public UnitOfWork(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            CharacterRepository = new CharacterRepository.CharacterRepository(databaseContext);
            UserRepository = new UserRepository.UserRepository(databaseContext);
        }

        public async Task SaveAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}