using FantasyFights.DAL.Entities;

namespace FantasyFights.DAL.Repositories.EmailVerificationCodeRepository
{
    public interface IEmailVerificationCodeRepository
    {
        Task<List<EmailVerificationCode>> GetAllEmailVerificationCodes();

        Task<EmailVerificationCode?> GetEmailVerificationCodeByOwnerId(int ownerId);

        Task<EmailVerificationCode> CreateEmailVerificationCode(EmailVerificationCode emailVerificationCode);
    }
}