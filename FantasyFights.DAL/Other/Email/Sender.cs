using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyFights.DAL.Other.Email
{
    public class Sender
    {
        public required string Address { get; set; }
        public required string Password { get; set; }
    }
}