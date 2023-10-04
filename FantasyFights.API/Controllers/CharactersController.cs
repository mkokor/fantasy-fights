using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FantasyFights.DAL.Models;
using FantasyFights.BLL.Services.CharactersService;
using FantasyFights.BLL.DTOs.Character;

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
        public ActionResult<List<CharacterResponseDto>> GetAllCharacters()
        {
            try
            {
                return Ok(_characterService.GetAllCharacters());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Something went wrong." });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<CharacterResponseDto> GetCharacter(string id)
        {
            try
            {
                return Ok(_characterService.GetCharacter(id));
            }
            catch (NullReferenceException exception)
            {
                return NotFound(new { exception.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Something went wrong." });
            }
        }
    }
}