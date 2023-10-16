namespace FantasyFights.BLL.DTOs.User
{
    public class UserLoginRequestDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}