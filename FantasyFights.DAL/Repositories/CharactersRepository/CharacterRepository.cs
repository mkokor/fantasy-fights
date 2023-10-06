using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FantasyFights.DAL.Repositories.CharactersRepository
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly DatabaseContext _databaseContext;

        public CharacterRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<List<Character>> GetAllCharacters()
        {
            return await _databaseContext.Characters.ToListAsync();
        }

        public async Task<Character?> GetCharacter(int id)
        {
            return await _databaseContext.Characters.FindAsync(id);
        }

        public async Task<Character> CreateCharacter(Character character)
        {
            await _databaseContext.Characters.AddAsync(character);
            return character;
        }
    }
}