using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Models;

namespace FantasyFights.BLL.Services.CharactersService
{
    public interface ICharactersService
    {
        List<Character> GetAllCharacters();
        Character GetCharacter(string id);
    }
}