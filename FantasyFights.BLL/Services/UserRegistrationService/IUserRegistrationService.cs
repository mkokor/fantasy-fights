using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.BLL.DTOs.User;

namespace FantasyFights.BLL.Services.UserRegistrationService
{
    public interface IUserRegistrationService
    {
        Task<UserResponseDto> RegisterUser(UserRegistrationRequestDto userRegistrationRequestDto);
    }
}