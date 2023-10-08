using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Entities;

namespace FantasyFights.DAL.Repositories.EmailVerificationCodeRepository
{
    public interface IEmailVerificationCodeRepository
    {
        Task<List<EmailVerificationCode>> GetAllEmailVerificationCodes();

        Task<EmailVerificationCode> CreateEmailVerificationCode(EmailVerificationCode emailVerificationCode);
    }
}