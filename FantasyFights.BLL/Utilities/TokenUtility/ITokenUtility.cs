using FantasyFights.DAL.Entities;

namespace FantasyFights.BLL.Utilities.TokenUtility
{
    public interface ITokenUtility
    {
        string GenerateAccessToken(User user);

        Tuple<string, RefreshToken> GenerateRefreshToken(User user);
    }
}