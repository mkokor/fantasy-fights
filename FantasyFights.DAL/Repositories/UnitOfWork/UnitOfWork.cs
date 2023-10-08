using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Repositories.CharacterRepository;
using FantasyFights.DAL.Repositories.EmailVerificationCodeRepository;
using FantasyFights.DAL.Repositories.UserRepository;

namespace FantasyFights.DAL.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _databaseContext;

        public ICharacterRepository CharacterRepository { get; init; }
        public IUserRepository UserRepository { get; init; }
        public IEmailVerificationCodeRepository EmailVerificationCodeRepository { get; init; }

        public UnitOfWork(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            CharacterRepository = new CharacterRepository.CharacterRepository(databaseContext);
            UserRepository = new UserRepository.UserRepository(databaseContext);
            EmailVerificationCodeRepository = new EmailVerificationCodeRepository.EmailVerificationCodeRepository(databaseContext);
        }

        public async Task SaveAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}