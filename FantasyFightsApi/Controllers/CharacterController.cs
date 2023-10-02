using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFightsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FantasyFightsApi.Controllers
{
    [ApiController]
    [Route("/api/character")]
    public class CharacterController : ControllerBase
    {
        private static Character myCharacter = new Character();

        [HttpGet]
        public ActionResult<Character> GetCharacter()
        {
            return Ok(myCharacter);
        }
    }
}