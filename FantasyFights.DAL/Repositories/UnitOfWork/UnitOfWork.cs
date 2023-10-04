using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Models;
using FantasyFights.DAL.Repositories.CharactersRepository;

namespace FantasyFights.DAL.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private static List<Character> characters = new()
        {
            new() {
                Name = "Asterix"
            },
            new() {
                Id = 1,
                Name = "Obelix"
            }
        };

        public ICharacterRepository CharacterRepository { get; }

        public UnitOfWork()
        {
            CharacterRepository = new CharacterRepository(characters);
        }
    }
}