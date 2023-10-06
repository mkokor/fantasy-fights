using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyFights.BLL.DTOs.User
{
    public class UserRegistrationRequestDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}