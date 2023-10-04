using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FantasyFights.BLL.DTOs.Character;
using FantasyFights.DAL.Models;
using FantasyFights.DAL.Repositories.CharactersRepository;
using FantasyFights.DAL.Repositories.UnitOfWork;

namespace FantasyFights.BLL.Services.CharactersService
{
    public class CharactersService : ICharactersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CharactersService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<CharacterResponseDto> GetAllCharacters()
        {
            var characters = _unitOfWork.CharacterRepository.GetAllCharacters();
            return _mapper.Map<List<CharacterResponseDto>>(characters);
        }

        public CharacterResponseDto GetCharacter(string id)
        {
            var character = _unitOfWork.CharacterRepository.GetCharacter(id) ?? throw new NullReferenceException("Character with provided id does not exist.");
            return _mapper.Map<CharacterResponseDto>(character);
        }

        public CharacterResponseDto CreateCharacter(CharacterRequestDto character)
        {
            if (character.Name is null)
                throw new ArgumentNullException("Character name field is required.");
            var newCharacter = _mapper.Map<Character>(character);
            _unitOfWork.CharacterRepository.CreateCharacter(newCharacter);
            return _mapper.Map<CharacterResponseDto>(newCharacter);
        }
    }
}