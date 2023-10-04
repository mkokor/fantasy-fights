using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Models;

namespace FantasyFights.DAL.Repositories.CharactersRepository
{
    public class CharacterRepository : ICharacterRepository
    {
        private static List<Character> _characters = new();

        public CharacterRepository(List<Character> characters)
        {
            _characters = characters;
        }

        public List<Character> GetAllCharacters()
        {
            return _characters;
        }

        public Character? GetCharacter(string id)
        {
            return _characters.Find(character => id.Equals($"{character.Id}"));
        }

        public Character CreateCharacter(Character character)
        {
            character.Id = _characters.Count;
            _characters.Add(character);
            return character;
        }
    }
}