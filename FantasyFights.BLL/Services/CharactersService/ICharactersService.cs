using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.BLL.DTOs.Character;
using FantasyFights.DAL.Models;

namespace FantasyFights.BLL.Services.CharactersService
{
    public interface ICharactersService
    {
        List<CharacterResponseDto> GetAllCharacters();

        CharacterResponseDto GetCharacter(string id);
    }
}