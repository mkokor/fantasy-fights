using System.Text.Json.Serialization;

namespace FantasyFightsApi.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CharacterClass
    {
        Knight = 1,
        Mage = 2,
        Cleric = 3
    }
}