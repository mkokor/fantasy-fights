using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Models;

namespace FantasyFights.DAL.Repositories.CharactersRepository
{
    public interface ICharacterRepository
    {
        Task<List<Character>> GetAllCharacters();

        Task<Character?> GetCharacter(int id);

        Task<Character> CreateCharacter(Character character);
    }
}