using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FantasyFights.BLL.Services.CharactersService;
using FantasyFights.BLL.DTOs.Character;
using System.ComponentModel.DataAnnotations;

namespace FantasyFights.API.Controllers
{
    [ApiController]
    [Route("api/characters")]
    public class CharactersController : ControllerBase
    {
        private readonly ICharactersService _characterService;

        public CharactersController(ICharactersService charactersService)
        {
            _characterService = charactersService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CharacterResponseDto>>> GetAllCharacters()
        {
            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterResponseDto>> GetCharacter([Required] string id)
        {
            return Ok(await _characterService.GetCharacter(id));
        }

        [HttpPost]
        public async Task<ActionResult<CharacterResponseDto>> CreateCharacter([Required, FromBody] CharacterRequestDto character)
        {
            return Ok(await _characterService.CreateCharacter(character));
        }
    }
}