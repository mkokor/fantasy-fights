using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Entities;

namespace FantasyFights.DAL.Repositories.CharacterRepository
{
    public interface ICharacterRepository
    {
        Task<List<Character>> GetAllCharacters();

        Task<Character?> GetCharacter(int id);

        Task<Character> CreateCharacter(Character character);
    }
}