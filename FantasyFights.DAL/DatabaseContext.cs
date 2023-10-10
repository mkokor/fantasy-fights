using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FantasyFights.DAL
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; private set; }
        public DbSet<Character> Characters { get; private set; }
        public DbSet<EmailConfirmationCode> EmailConfirmationCodes { get; private set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    }
}