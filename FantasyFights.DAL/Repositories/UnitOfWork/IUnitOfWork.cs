using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Repositories.CharacterRepository;
using FantasyFights.DAL.Repositories.UserRepository;

namespace FantasyFights.DAL.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICharacterRepository CharacterRepository { get; init; }
        IUserRepository UserRepository { get; init; }

        Task SaveAsync();
    }
}