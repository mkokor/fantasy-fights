using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.BLL.DTOs.Character;

namespace FantasyFights.BLL.Services.CharactersService
{
    public interface ICharactersService
    {
        Task<List<CharacterResponseDto>> GetAllCharacters();

        Task<CharacterResponseDto> GetCharacter(string id);

        Task<CharacterResponseDto> CreateCharacter(CharacterRequestDto character);
    }
}