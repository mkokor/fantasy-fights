using System.Text.RegularExpressions;
using AutoMapper;
using FantasyFights.BLL.DTOs.EmailConfirmation;
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

        private async Task<string> CreateOrUpdateEmailConfirmationCode(User user)
        {
            var randomNumberGenerator = new Random();
            var emailConfirmationCodeValue = $"{randomNumberGenerator.Next(100000, 999999)}";
            var emailConfirmationCode = await _unitOfWork.EmailConfirmationCodeRepository.GetEmailConfirmationCodeByOwnerId(user.Id);
            if (emailConfirmationCode is null)
                await _unitOfWork.EmailConfirmationCodeRepository.CreateEmailConfirmationCode(new EmailConfirmationCode
                {
                    ValueHash = CryptoUtility.Hash(emailConfirmationCodeValue),
                    ExpirationDateAndTime = DateTime.Now.AddMinutes(15),
                    OwnerId = user.Id
                });
            else
            {
                emailConfirmationCode.ValueHash = CryptoUtility.Hash(emailConfirmationCodeValue);
                emailConfirmationCode.ExpirationDateAndTime = DateTime.Now.AddMinutes(15);
            }
            await _unitOfWork.SaveAsync();
            return emailConfirmationCodeValue;
        }

        private void VerifyEmailConfirmationCode(string acceptedValue, EmailConfirmationCode realData)
        {
            if (!CryptoUtility.Compare(realData.ValueHash, acceptedValue))
                throw new ArgumentException("Confirmation code is incorrect.");
            if (realData.ExpirationDateAndTime < DateTime.Now)
                throw new ArgumentException("Confirmation code has expired.");
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
            await SendConfirmationEmail(user.Email);
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task SendConfirmationEmail(string email)
        {
            var recipient = await _unitOfWork.UserRepository.GetUserByEmail(email) ?? throw new NullReferenceException("User with provided email does not exist.");
            var emailConfirmationCode = await CreateOrUpdateEmailConfirmationCode(recipient);
            EmailUtility.SendEmail(ConfigurateEmailData(new List<Recipient> { new() { Address = recipient.Email } }, "Account Confirmation", $"Confirmation code: {emailConfirmationCode}"));
        }

        public async Task ConfirmEmail(EmailConfirmationRequestDto emailConfirmationRequestDto)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmail(emailConfirmationRequestDto.Email) ?? throw new NullReferenceException("User with provided email does not exist.");
            var emailConfirmationCode = await _unitOfWork.EmailConfirmationCodeRepository.GetEmailConfirmationCodeByOwnerId(user.Id) ?? throw new NullReferenceException("Confirmation code could not be found.");
            VerifyEmailConfirmationCode(emailConfirmationRequestDto.ConfirmationCode, emailConfirmationCode);
            user.EmailConfirmed = true;
            _unitOfWork.EmailConfirmationCodeRepository.DeleteEmailConfirmationCode(emailConfirmationCode);
            await _unitOfWork.SaveAsync();
        }

        [GeneratedRegex("^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{8,}$")]
        private static partial Regex PasswordStrengthValidation();
    }
}
