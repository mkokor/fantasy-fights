using System.ComponentModel.DataAnnotations;
using FantasyFights.BLL.DTOs.EmailConfirmation;
using FantasyFights.BLL.DTOs.User;
using FantasyFights.BLL.Services.AuthenticationService;
using FantasyFights.BLL.Services.UserRegistrationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FantasyFights.API.Controllers
{
    [Route("api/auth")]
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
            return await _userRegistrationService.RegisterUser(userRegistrationRequestDto);
        }

        [HttpPost("email/confirmation")]
        public async Task<ActionResult> ConfirmEmail([FromBody, Required] EmailConfirmationRequestDto emailConfirmationRequestDto)
        {
            await _userRegistrationService.ConfirmEmail(emailConfirmationRequestDto);
            return Ok(new { Message = "Email successfully confirmed." });
        }

        [HttpPost("email/confirmation-code-refresh")]
        public async Task<ActionResult> SendConfirmationCodeEmail([FromBody, Required] EmailConfirmationCodeRequestDto emailConfirmationCodeRequestDto)
        {
            await _userRegistrationService.SendConfirmationEmail(emailConfirmationCodeRequestDto.Email);
            return Ok(new { Message = "Confirmation code email successfully sent." });
        }
    }
}