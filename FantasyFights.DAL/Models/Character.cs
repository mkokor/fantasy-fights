using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Other;

namespace FantasyFights.DAL.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int HitPoints { get; set; } = 0;
        public int Strength { get; set; } = 0;
        public int Defence { get; set; } = 0;
        public int Intelligence { get; set; } = 0;
        public CharacterClass Class { get; set; } = CharacterClass.Knight;
    }
}