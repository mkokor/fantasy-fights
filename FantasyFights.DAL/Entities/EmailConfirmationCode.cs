using System.ComponentModel.DataAnnotations.Schema;

namespace FantasyFights.DAL.Entities
{
    public class EmailConfirmationCode
    {
        public int Id { get; set; }
        public required string ValueHash { get; set; }
        public required DateTime ExpirationDateAndTime { get; set; }

        [ForeignKey("User")]
        public int OwnerId { get; set; }
        public User? Owner { get; set; }
    }
}