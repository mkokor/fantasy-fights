using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Models;
using FantasyFights.DAL.Repositories.CharactersRepository;

namespace FantasyFights.BLL.Services.CharactersService
{
    public class CharactersService : ICharactersService
    {
        private readonly ICharacterRepository _characterRepository;

        public CharactersService(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }

        public List<Character> GetAllCharacters()
        {
            return _characterRepository.GetAllCharacters();
        }

        public Character GetCharacter(string id)
        {
            var character = _characterRepository.GetCharacter(id) ?? throw new NullReferenceException("Character with provided id does not exist.");
            return character;
        }
    }
}