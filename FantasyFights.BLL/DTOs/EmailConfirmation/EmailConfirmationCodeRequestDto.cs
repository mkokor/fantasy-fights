using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyFights.BLL.DTOs.EmailConfirmation
{
    public class EmailConfirmationCodeRequestDto
    {
        public required string Email { get; set; }
    }
}