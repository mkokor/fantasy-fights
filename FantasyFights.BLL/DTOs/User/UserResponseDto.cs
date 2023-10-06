using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyFights.BLL.DTOs.User
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public required string Username { get; set; }
    }
}