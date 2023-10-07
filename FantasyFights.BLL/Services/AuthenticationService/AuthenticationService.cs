using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using FantasyFights.BLL.DTOs.User;
using FantasyFights.BLL.Utilities;
using FantasyFights.DAL.Entities;
using FantasyFights.DAL.Repositories.UnitOfWork;

namespace FantasyFights.BLL.Services.AuthenticationService
{
    public partial class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [GeneratedRegex("^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{8,}$")]
        private static partial Regex PasswordStrengthValidationRegex();

        private static void ValidatePasswordStrength(string password)
        {
            // minimum 8 characters
            // minimum one digit
            // minimum one special character
            if (PasswordStrengthValidationRegex().IsMatch(password))
                return;
            throw new ArgumentException("Password needs to contain minimum of 8 characters, including one digit and one special character.");
        }

        private async Task ValidateEmail(string email)
        {
            if (!EmailUtility.IsValidEmail(email))
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

        public async Task<UserResponseDto> RegisterUser(UserRegistrationRequestDto userRegistrationRequestDto)
        {
            await ValidateEmail(userRegistrationRequestDto.Email);
            await ValidateUsername(userRegistrationRequestDto.Username);
            ValidatePasswordStrength(userRegistrationRequestDto.Password);
            // Send confirmation email message!
            var user = _mapper.Map<User>(userRegistrationRequestDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRegistrationRequestDto.Password);
            await _unitOfWork.UserRepository.CreateUser(user);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<UserResponseDto>(user);
        }
    }
}