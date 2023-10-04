using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Other;

namespace FantasyFights.BLL.DTOs.Character
{
    public class CharacterRequestDto
    {
        public string? Name { get; set; }
        public int? HitPoints { get; set; }
        public int? Strength { get; set; }
        public int? Defence { get; set; }
        public int? Intelligence { get; set; }
        public CharacterClass? Class { get; set; } = CharacterClass.Knight;
    }
}