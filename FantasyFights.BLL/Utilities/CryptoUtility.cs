using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

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
    }
}