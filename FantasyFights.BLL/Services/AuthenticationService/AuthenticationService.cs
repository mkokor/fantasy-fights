using AutoMapper;
using FantasyFights.BLL.DTOs.User;
using FantasyFights.BLL.Exceptions;
using FantasyFights.BLL.Utilities;
using FantasyFights.BLL.Utilities.TokenUtility;
using FantasyFights.DAL.Repositories.UnitOfWork;

namespace FantasyFights.BLL.Services.AuthenticationService
{
    public partial class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenUtility _tokenUtility;

        public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper, ITokenUtility tokenUtility)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenUtility = tokenUtility;
        }

        public async Task<UserLoginResponseDto> LogInUser(UserLoginRequestDto userLoginRequestDto)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsername(userLoginRequestDto.Username) ?? throw new NotFoundException("User with provided username could not be found.");
            if (!user.EmailConfirmed)
                throw new BadRequestException("Email is not confirmed.");
            if (!CryptoUtility.Compare(userLoginRequestDto.Password, user.PasswordHash))
                throw new BadRequestException("Password does not match the username.");
            return new UserLoginResponseDto
            {
                AccessToken = _tokenUtility.GenerateAccessToken(user)
            };
        }
    }
}