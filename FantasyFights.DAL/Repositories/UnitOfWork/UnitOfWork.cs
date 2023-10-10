using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Repositories.CharacterRepository;
using FantasyFights.DAL.Repositories.EmailConfirmationCodeRepository;
using FantasyFights.DAL.Repositories.UserRepository;

namespace FantasyFights.DAL.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _databaseContext;

        public ICharacterRepository CharacterRepository { get; init; }
        public IUserRepository UserRepository { get; init; }
        public IEmailConfirmationCodeRepository EmailConfirmationCodeRepository { get; init; }

        public UnitOfWork(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            CharacterRepository = new CharacterRepository.CharacterRepository(databaseContext);
            UserRepository = new UserRepository.UserRepository(databaseContext);
            EmailConfirmationCodeRepository = new EmailConfirmationCodeRepository.EmailConfirmationCodeRepository(databaseContext);
        }

        public async Task SaveAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}