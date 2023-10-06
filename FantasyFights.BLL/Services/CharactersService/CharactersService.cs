using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FantasyFights.BLL.DTOs.Character;
using FantasyFights.DAL.Entities;
using FantasyFights.DAL.Repositories.CharacterRepository;
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

        public async Task<List<CharacterResponseDto>> GetAllCharacters()
        {
            var characters = await _unitOfWork.CharacterRepository.GetAllCharacters();
            return _mapper.Map<List<CharacterResponseDto>>(characters);
        }

        public async Task<CharacterResponseDto> GetCharacter(string id)
        {
            try
            {
                var character = await _unitOfWork.CharacterRepository.GetCharacter(int.Parse(id)) ?? throw new NullReferenceException("Character with provided id does not exist.");
                return _mapper.Map<CharacterResponseDto>(character);
            }
            catch (FormatException)
            {
                throw new NullReferenceException("Character with provided id does not exist.");
            }
        }

        public async Task<CharacterResponseDto> CreateCharacter(CharacterRequestDto character)
        {
            if (character.Name is null)
                throw new ArgumentNullException("Character name field is required.");
            var newCharacter = _mapper.Map<Character>(character);
            await _unitOfWork.CharacterRepository.CreateCharacter(newCharacter);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<CharacterResponseDto>(newCharacter);
        }
    }
}