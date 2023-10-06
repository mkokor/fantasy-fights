using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FantasyFights.DAL
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Character> Characters { get; private set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    }
}