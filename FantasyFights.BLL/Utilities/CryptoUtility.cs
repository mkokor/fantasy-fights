using System.Security.Cryptography;

namespace FantasyFights.BLL.Utilities
{
    public class CryptoUtility
    {
        public static string Hash(string value)
        {
            return BCrypt.Net.BCrypt.HashPassword(value);
        }

        public static string GenerateRandomString()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        public static bool Compare(string value, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(value, hash);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}