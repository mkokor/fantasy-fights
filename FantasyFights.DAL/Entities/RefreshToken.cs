using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyFights.DAL.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public required string ValueHash { get; set; }

        public required DateTime ExpirationDateAndTime { get; set; }

        [ForeignKey("User")]
        public int OwnerId { get; set; }
        public User? Owner { get; set; }
    }
}