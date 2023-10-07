using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyFights.BLL.Utilities
{
    public class EnvironmentUtility
    {
        public static string GetEnvironmentVariable(string key)
        {
            return Environment.GetEnvironmentVariable(key) ?? throw new Exception("Unable to read environment variable value with provided key.");
        }
    }
}