using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Models;

namespace FantasyFights.DAL.Repositories.CharactersRepository
{
    public interface ICharacterRepository
    {
        List<Character> GetAllCharacters();

        Character? GetCharacter(string id);
    }
}