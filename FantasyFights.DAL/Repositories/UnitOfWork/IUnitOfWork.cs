using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Repositories.CharactersRepository;

namespace FantasyFights.DAL.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICharacterRepository CharacterRepository { get; }

        Task SaveAsync();
    }
}