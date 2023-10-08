using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyFights.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; } = false;
    }
}