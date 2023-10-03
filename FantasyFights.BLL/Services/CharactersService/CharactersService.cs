using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Models;
using FantasyFights.DAL.Repositories.CharactersRepository;
using FantasyFights.DAL.Repositories.UnitOfWork;

namespace FantasyFights.BLL.Services.CharactersService
{
    public class CharactersService : ICharactersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CharactersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Character> GetAllCharacters()
        {
            return _unitOfWork.CharacterRepository.GetAllCharacters();
        }

        public Character GetCharacter(string id)
        {
            var character = _unitOfWork.CharacterRepository.GetCharacter(id) ?? throw new NullReferenceException("Character with provided id does not exist.");
            return character;
        }
    }
}