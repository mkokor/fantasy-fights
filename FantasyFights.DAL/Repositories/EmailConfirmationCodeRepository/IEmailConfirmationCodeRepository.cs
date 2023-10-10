using FantasyFights.DAL.Entities;

namespace FantasyFights.DAL.Repositories.EmailConfirmationCodeRepository
{
    public interface IEmailConfirmationCodeRepository
    {
        Task<List<EmailConfirmationCode>> GetAllEmailConfirmationCodes();

        Task<EmailConfirmationCode?> GetEmailConfirmationCodeByOwnerId(int ownerId);

        Task<EmailConfirmationCode> CreateEmailConfirmationCode(EmailConfirmationCode emailConfirmationCode);

        void DeleteEmailConfirmationCode(EmailConfirmationCode emailConfirmationCode);
    }
}