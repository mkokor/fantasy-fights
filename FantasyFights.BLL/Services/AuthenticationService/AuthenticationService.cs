using AutoMapper;
using FantasyFights.BLL.DTOs.User;
using FantasyFights.BLL.Exceptions;
using FantasyFights.BLL.Utilities;
using FantasyFights.BLL.Utilities.TokenUtility;
using FantasyFights.DAL.Entities;
using FantasyFights.DAL.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Http;

namespace FantasyFights.BLL.Services.AuthenticationService
{
    public partial class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenUtility _tokenUtility;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper, ITokenUtility tokenUtility, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenUtility = tokenUtility;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<Tuple<string, RefreshToken>> CreateRefreshToken(User user)
        {
            var refreshToken = _tokenUtility.GenerateRefreshToken(user);
            await _unitOfWork.RefreshTokenRepository.CreateRefreshToken(refreshToken.Item2);
            await _unitOfWork.SaveAsync();
            return new Tuple<string, RefreshToken>(refreshToken.Item1, refreshToken.Item2);
        }

        private void SetRefreshTokenInHttpOnlyCookie(Tuple<string, RefreshToken> refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Item2.ExpirationDateAndTime,
                SameSite = SameSiteMode.None,
                Secure = true
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refresh-token", refreshToken.Item1, cookieOptions);
        }

        public async Task<UserLoginResponseDto> LogInUser(UserLoginRequestDto userLoginRequestDto)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsername(userLoginRequestDto.Username) ?? throw new NotFoundException("User with provided username could not be found.");
            if (!user.EmailConfirmed)
                throw new BadRequestException("Email is not confirmed.");
            if (!CryptoUtility.Compare(userLoginRequestDto.Password, user.PasswordHash))
                throw new BadRequestException("Password does not match the username.");
            SetRefreshTokenInHttpOnlyCookie(await CreateRefreshToken(user));
            return new UserLoginResponseDto
            {
                AccessToken = _tokenUtility.GenerateAccessToken(user)
            };
        }
    }
}