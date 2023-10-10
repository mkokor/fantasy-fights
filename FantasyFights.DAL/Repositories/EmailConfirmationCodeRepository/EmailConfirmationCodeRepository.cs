using FantasyFights.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FantasyFights.DAL.Repositories.EmailConfirmationCodeRepository
{
    public class EmailConfirmationCodeRepository : IEmailConfirmationCodeRepository
    {
        private readonly DatabaseContext _databaseContext;

        public EmailConfirmationCodeRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<EmailConfirmationCode> CreateEmailConfirmationCode(EmailConfirmationCode emailConfirmationCode)
        {
            await _databaseContext.EmailConfirmationCodes.AddAsync(emailConfirmationCode);
            return emailConfirmationCode;
        }

        public async Task<List<EmailConfirmationCode>> GetAllEmailConfirmationCodes()
        {
            return await _databaseContext.EmailConfirmationCodes.Include(code => code.Owner)
                                                                .ToListAsync();
        }

        public async Task<EmailConfirmationCode?> GetEmailConfirmationCodeByOwnerId(int ownerId)
        {
            return await _databaseContext.EmailConfirmationCodes.Include(code => code.Owner)
                                                                .FirstOrDefaultAsync(code => code.OwnerId == ownerId);
        }

        public void DeleteEmailConfirmationCode(EmailConfirmationCode emailConfirmationCode)
        {
            _databaseContext.EmailConfirmationCodes.Remove(emailConfirmationCode);
        }
    }
}