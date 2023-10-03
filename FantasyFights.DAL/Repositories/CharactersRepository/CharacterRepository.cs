using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Models;

namespace FantasyFights.DAL.Repositories.CharactersRepository
{
    public class CharacterRepository : ICharacterRepository
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

        public Character? GetCharacter(string id)
        {
            return characters.Find(character => id.Equals($"{character.Id}"));
        }
    }
}