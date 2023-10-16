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
        public DbSet<RefreshToken> RefreshTokens { get; private set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region User
            modelBuilder.Entity<User>()
                .ToTable("users");

            modelBuilder.Entity<User>()
                .Property(user => user.Id)
                .HasColumnName("id");

            modelBuilder.Entity<User>()
                .Property(user => user.Email)
                .HasColumnName("email");

            modelBuilder.Entity<User>()
                .Property(user => user.Username)
                .HasColumnName("username");

            modelBuilder.Entity<User>()
                .Property(user => user.PasswordHash)
                .HasColumnName("password_hash");
            #endregion

            #region RefreshToken
            modelBuilder.Entity<RefreshToken>()
                .ToTable("refresh_tokens");

            modelBuilder.Entity<RefreshToken>()
                .Property(refreshToken => refreshToken.Id)
                .HasColumnName("id");

            modelBuilder.Entity<RefreshToken>()
                .Property(refreshToken => refreshToken.ValueHash)
                .HasColumnName("value_hash");

            modelBuilder.Entity<RefreshToken>()
                .Property(refreshToken => refreshToken.ExpirationDateAndTime)
                .HasColumnName("expiration_date_and_time");

            modelBuilder.Entity<RefreshToken>()
                .Property(refreshToken => refreshToken.OwnerId)
                .HasColumnName("owner_id");
            #endregion

            #region EmailConfirmationCode
            modelBuilder.Entity<EmailConfirmationCode>()
                .ToTable("email_confirmation_codes");

            modelBuilder.Entity<EmailConfirmationCode>()
                .Property(emailConfirmationCode => emailConfirmationCode.Id)
                .HasColumnName("id");

            modelBuilder.Entity<EmailConfirmationCode>()
                .Property(emailConfirmationCode => emailConfirmationCode.ValueHash)
                .HasColumnName("value_hash");

            modelBuilder.Entity<EmailConfirmationCode>()
                .Property(emailConfirmationCode => emailConfirmationCode.ExpirationDateAndTime)
                .HasColumnName("expiration_date_and_time");

            modelBuilder.Entity<EmailConfirmationCode>()
                .Property(emailConfirmationCode => emailConfirmationCode.OwnerId)
                .HasColumnName("owner_id");
            #endregion

            #region Character
            modelBuilder.Entity<Character>()
                .ToTable("character");

            modelBuilder.Entity<Character>()
                .Property(character => character.Id)
                .HasColumnName("id");

            modelBuilder.Entity<Character>()
                .Property(character => character.Name)
                .HasColumnName("name");

            modelBuilder.Entity<Character>()
                .Property(character => character.Strength)
                .HasColumnName("strength");

            modelBuilder.Entity<Character>()
                .Property(character => character.Class)
                .HasColumnName("class");

            modelBuilder.Entity<Character>()
                .Property(character => character.Defence)
                .HasColumnName("defence");

            modelBuilder.Entity<Character>()
                .Property(character => character.HitPoints)
                .HasColumnName("hit_points");

            modelBuilder.Entity<Character>()
                .Property(character => character.Intelligence)
                .HasColumnName("intelligence");
            #endregion
        }
    }
}