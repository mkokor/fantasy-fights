using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Repositories.CharacterRepository;
using FantasyFights.DAL.Repositories.EmailConfirmationCodeRepository;
using FantasyFights.DAL.Repositories.RefreshTokenRepository;
using FantasyFights.DAL.Repositories.UserRepository;

namespace FantasyFights.DAL.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICharacterRepository CharacterRepository { get; init; }
        IUserRepository UserRepository { get; init; }
        IEmailConfirmationCodeRepository EmailConfirmationCodeRepository { get; init; }
        IRefreshTokenRepository RefreshTokenRepository { get; init; }

        Task SaveAsync();
    }
}