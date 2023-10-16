using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FantasyFights.DAL.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FantasyFights.BLL.Utilities.TokenUtility
{
    public class TokenUtility : ITokenUtility
    {
        private readonly IConfiguration _configuration;

        public TokenUtility(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region JsonWebToken
        private static SymmetricSecurityKey GetSecretKey(string secret)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }

        private static SigningCredentials GetDigitalSignature(string secret)
        {
            // Algorithm is HMAC (symmetric), so the key is secret (not private).
            return new SigningCredentials(GetSecretKey(secret), SecurityAlgorithms.HmacSha512Signature);
        }

        private JwtSecurityToken ConfigureJwt(List<Claim> claims, string secret)
        {
            return new JwtSecurityToken(
                issuer: _configuration["Authentication:AccessToken:Issuer"] ?? throw new Exception("JSON Web Token issuer name could not be found."),
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: GetDigitalSignature(secret));
        }

        private string GenerateJwt(List<Claim> claims, string secret)
        {
            return new JwtSecurityTokenHandler().WriteToken(ConfigureJwt(claims, secret));
        }
        #endregion

        #region AccessToken
        private static List<Claim> GetAccessTokenClaims(User user)
        {
            return new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, $"{user.Id}"), // Subject identifier will be user identification number!
                new(ClaimTypes.Name, user.Username),
            };
        }

        public string GenerateAccessToken(User user)
        {
            var secret = _configuration["Authentication:AccessToken:Secret"] ?? throw new Exception("Access token secret could not be found.");
            return GenerateJwt(GetAccessTokenClaims(user), secret);
        }
        #endregion

        #region RefreshToken
        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
        #endregion
    }
}