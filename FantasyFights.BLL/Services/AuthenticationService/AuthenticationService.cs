using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FantasyFights.BLL.DTOs.User;
using FantasyFights.DAL.Entities;
using FantasyFights.DAL.Repositories.UnitOfWork;

namespace FantasyFights.BLL.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> RegisterUser(UserRegistrationRequestDto userRegistrationRequestDto)
        {
            if (userRegistrationRequestDto.Username is null || userRegistrationRequestDto.Password is null)
                throw new NullReferenceException("Username and password are required fields.");
            var user = _mapper.Map<User>(userRegistrationRequestDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRegistrationRequestDto.Password);
            await _unitOfWork.UserRepository.CreateUser(user);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<UserResponseDto>(user);
        }
    }
}