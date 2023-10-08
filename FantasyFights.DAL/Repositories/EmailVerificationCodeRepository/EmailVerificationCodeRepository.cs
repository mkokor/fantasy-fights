using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FantasyFights.DAL.Repositories.EmailVerificationCodeRepository
{
    public class EmailVerificationCodeRepository : IEmailVerificationCodeRepository
    {
        private readonly DatabaseContext _databaseContext;

        public EmailVerificationCodeRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<EmailVerificationCode> CreateEmailVerificationCode(EmailVerificationCode emailVerificationCode)
        {
            await _databaseContext.EmailVerificationCodes.AddAsync(emailVerificationCode);
            return emailVerificationCode;
        }

        public async Task<List<EmailVerificationCode>> GetAllEmailVerificationCodes()
        {
            return await _databaseContext.EmailVerificationCodes.ToListAsync();
        }
    }
}