using System.Text.RegularExpressions;
using AutoMapper;
using FantasyFights.BLL.DTOs.User;
using FantasyFights.BLL.Utilities;
using FantasyFights.DAL.Entities;
using FantasyFights.DAL.Other.Email;
using FantasyFights.DAL.Repositories.UnitOfWork;

namespace FantasyFights.BLL.Services.UserRegistrationService
{
    public partial class UserRegistrationService : IUserRegistrationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserRegistrationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private async Task ValidateEmail(string email)
        {
            if (!EmailUtility.IsValid(email))
                throw new ArgumentException("Format of provided email is not recognized.");
            var allUsers = await _unitOfWork.UserRepository.GetAllUsers();
            if (allUsers.FirstOrDefault(user => user.Email.Equals(email)) != null)
                throw new ArgumentException("Provided email is not available.");
        }

        private async Task ValidateUsername(string username)
        {
            if (!(username.Length >= 3 && username.Any(char.IsLetter)))
                throw new ArgumentException("Username needs to contain minimum of 3 characters, including one letter.");
            var allUsers = await _unitOfWork.UserRepository.GetAllUsers();
            if (allUsers.FirstOrDefault(user => user.Username.Equals(username)) != null)
                throw new ArgumentException("Provided username is not available.");
        }

        private static void ValidatePasswordStrength(string password)
        {
            // minimum 8 characters
            // minimum one digit
            // minimum one special character
            if (PasswordStrengthValidation().IsMatch(password))
                return;
            throw new ArgumentException("Password needs to contain minimum of 8 characters, including one digit and one special character!");
        }

        private static EmailConfiguration ConfigurateEmailData(List<Recipient> recipients, string subject, string body)
        {
            // This method is customized for this application.
            return new EmailConfiguration
            {
                Host = EnvironmentUtility.GetEnvironmentVariable("EMAIL_HOST"),
                Port = int.Parse(EnvironmentUtility.GetEnvironmentVariable("EMAIL_PORT")),
                Sender = new Sender
                {
                    Address = EnvironmentUtility.GetEnvironmentVariable("EMAIL_ADDRESS"),
                    Password = EnvironmentUtility.GetEnvironmentVariable("EMAIL_PASSWORD")
                },
                Recipients = recipients,
                Subject = subject,
                Body = body
            };
        }

        private async Task<string> CreateOrUpdateEmailVerificationCode(User user)
        {
            var randomNumberGenerator = new Random();
            var emailVerificationCodeValue = $"{randomNumberGenerator.Next(100000, 999999)}";
            var emailVerificationCode = await _unitOfWork.EmailVerificationCodeRepository.GetEmailVerificationCodeByOwnerId(user.Id);
            if (emailVerificationCode is null)
                await _unitOfWork.EmailVerificationCodeRepository.CreateEmailVerificationCode(new EmailVerificationCode
                {
                    ValueHash = CryptoUtility.Hash(emailVerificationCodeValue),
                    ExpirationDateAndTime = DateTime.Now.AddMinutes(15),
                    OwnerId = user.Id
                });
            else
                emailVerificationCode.ValueHash = CryptoUtility.Hash(emailVerificationCodeValue);
            await _unitOfWork.SaveAsync();
            return emailVerificationCodeValue;
        }

        private async Task SendConfirmationEmail(User recipient)
        {
            var emailVerificationCode = await CreateOrUpdateEmailVerificationCode(recipient);
            EmailUtility.SendEmail(ConfigurateEmailData(new List<Recipient> { new() { Address = recipient.Email } }, "Account Confirmation", $"Verification token: {emailVerificationCode}"));
        }

        public async Task<UserResponseDto> RegisterUser(UserRegistrationRequestDto userRegistrationRequestDto)
        {
            await ValidateEmail(userRegistrationRequestDto.Email);
            await ValidateUsername(userRegistrationRequestDto.Username);
            ValidatePasswordStrength(userRegistrationRequestDto.Password);
            var user = _mapper.Map<User>(userRegistrationRequestDto);
            user.PasswordHash = CryptoUtility.Hash(userRegistrationRequestDto.Password);
            await _unitOfWork.UserRepository.CreateUser(user);
            await _unitOfWork.SaveAsync();
            await SendConfirmationEmail(user);
            return _mapper.Map<UserResponseDto>(user);
        }

        [GeneratedRegex("^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{8,}$")]
        private static partial Regex PasswordStrengthValidation();
    }
}
