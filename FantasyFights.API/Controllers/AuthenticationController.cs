using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.BLL.DTOs.User;
using FantasyFights.BLL.Services.AuthenticationService;
using FantasyFights.BLL.Services.UserRegistrationService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FantasyFights.API.Controllers
{
    [Route("api/authentication")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserRegistrationService _userRegistrationService;

        public AuthenticationController(IAuthenticationService authenticationService, IUserRegistrationService userRegistrationService)
        {
            _authenticationService = authenticationService;
            _userRegistrationService = userRegistrationService;
        }

        [HttpPost("sign-up")]
        public async Task<ActionResult<UserResponseDto>> RegisterUser([FromBody, Required] UserRegistrationRequestDto userRegistrationRequestDto)
        {
            try
            {
                return await _userRegistrationService.RegisterUser(userRegistrationRequestDto);
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new { exception.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Something went wrong." });
            }
        }
    }
}