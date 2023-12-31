using System.Text.RegularExpressions;
using AutoMapper;
using FantasyFights.BLL.DTOs.EmailConfirmation;
using FantasyFights.BLL.DTOs.User;
using FantasyFights.BLL.Exceptions;
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
                throw new BadRequestException("Format of provided email is not recognized.");
            var allUsers = await _unitOfWork.UserRepository.GetAllUsers();
            if (allUsers.FirstOrDefault(user => user.Email.Equals(email)) != null)
                throw new BadRequestException("Provided email is not available.");
        }

        private async Task ValidateUsername(string username)
        {
            if (!(username.Length >= 3 && username.Any(char.IsLetter)))
                throw new BadRequestException("Username needs to contain minimum of 3 characters, including one letter.");
            var allUsers = await _unitOfWork.UserRepository.GetAllUsers();
            if (allUsers.FirstOrDefault(user => user.Username.Equals(username)) != null)
                throw new BadRequestException("Provided username is not available.");
        }

        private static void ValidatePasswordStrength(string password)
        {
            // minimum 8 characters
            // minimum one digit
            // minimum one special character
            if (PasswordStrengthValidation().IsMatch(password))
                return;
            throw new BadRequestException("Password needs to contain minimum of 8 characters, including one digit and one special character.");
        }

        private async Task<string> CreateOrUpdateEmailConfirmationCode(User user)
        {
            var emailConfirmationCodeValue = EmailUtility.GenerateEmailConfirmationCode();
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
            if (!CryptoUtility.Compare(acceptedValue, realData.ValueHash))
                throw new BadRequestException("Confirmation code is incorrect.");
            if (realData.ExpirationDateAndTime < DateTime.Now)
                throw new BadRequestException("Confirmation code has expired.");
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
            var recipient = await _unitOfWork.UserRepository.GetUserByEmail(email) ?? throw new NotFoundException("User with provided email could not be found.");
            var emailConfirmationCode = await CreateOrUpdateEmailConfirmationCode(recipient);
            EmailUtility.SendEmail(EmailUtility.ConfigurateEmailData(new List<Recipient> { new() { Address = recipient.Email } }, "Account Confirmation", $"Confirmation code: {emailConfirmationCode}"));
        }

        public async Task ConfirmEmail(EmailConfirmationRequestDto emailConfirmationRequestDto)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmail(emailConfirmationRequestDto.Email) ?? throw new NotFoundException("User with provided email could not be found.");
            var emailConfirmationCode = await _unitOfWork.EmailConfirmationCodeRepository.GetEmailConfirmationCodeByOwnerId(user.Id) ?? throw new NotFoundException("Confirmation code could not be found.");
            VerifyEmailConfirmationCode(emailConfirmationRequestDto.ConfirmationCode, emailConfirmationCode);
            user.EmailConfirmed = true;
            _unitOfWork.EmailConfirmationCodeRepository.DeleteEmailConfirmationCode(emailConfirmationCode);
            await _unitOfWork.SaveAsync();
        }

        [GeneratedRegex("^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{8,}$")]
        private static partial Regex PasswordStrengthValidation();
    }
}
