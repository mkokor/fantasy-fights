using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Models;

namespace FantasyFights.BLL.Services.CharactersService
{
    public class CharactersService : ICharactersService
    {
        private static List<Character> characters = new List<Character>
        {
            new Character
            {
                Name = "Asterix"
            },
            new Character
            {
                Name = "Obelix"
            }
        };

        public List<Character> GetAllCharacters()
        {
            return characters;
        }

        public Character GetCharacter(string id)
        {
            try
            {
                return characters.First(character => id.Equals($"{character.Id}"));
            }
            catch (InvalidOperationException)
            {
                throw new NullReferenceException("Character with provided id does not exist.");
            }
        }
    }
}