using FantasyFights.DAL.Entities;

namespace FantasyFights.DAL.Repositories.RefreshTokenRepository
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> CreateRefreshToken(RefreshToken refreshToken);
    }
}