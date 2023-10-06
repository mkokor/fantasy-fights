using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using FantasyFights.BLL.DTOs.Character;
using FantasyFights.DAL.Entities;

namespace FantasyFights.BLL
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, CharacterResponseDto>();
            CreateMap<CharacterRequestDto, Character>();
        }
    }
}