using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Entities;

namespace FantasyFights.DAL.Repositories.RefreshTokenRepository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DatabaseContext _databaseContext;

        public RefreshTokenRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<RefreshToken> CreateRefreshToken(RefreshToken refreshToken)
        {
            await _databaseContext.RefreshTokens.AddAsync(refreshToken);
            return refreshToken;
        }
    }
}