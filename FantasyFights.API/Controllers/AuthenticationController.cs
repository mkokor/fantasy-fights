using System.ComponentModel.DataAnnotations;
using FantasyFights.BLL.DTOs.EmailConfirmation;
using FantasyFights.BLL.DTOs.User;
using FantasyFights.BLL.Services.AuthenticationService;
using FantasyFights.BLL.Services.UserRegistrationService;
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

        [HttpPost("email/confirmation")]
        public async Task<ActionResult> ConfirmEmail([FromBody, Required] EmailConfirmationRequestDto emailConfirmationRequestDto)
        {
            try
            {
                await _userRegistrationService.ConfirmEmail(emailConfirmationRequestDto);
                return Ok(new { Message = "Email successfully confirmed." });
            }
            catch (NullReferenceException exception)
            {
                return NotFound(new { exception.Message });
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

        [HttpPost("email/confirmation-code-refresh")]
        public async Task<ActionResult> SendConfirmationCodeEmail([FromBody, Required] EmailConfirmationCodeRequestDto emailConfirmationCodeRequestDto)
        {
            try
            {
                await _userRegistrationService.SendConfirmationEmail(emailConfirmationCodeRequestDto.Email);
                return Ok(new { Message = "Confirmation code email successfully sent." });
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