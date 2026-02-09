using Auth.Dtos;
using Auth.Interfaces;
using Auth.Models;

namespace Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHashService _hashService;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthService(IHashService hashService, IUserService userService, ITokenService tokenService) 
        {
            _hashService = hashService;
            _userService = userService;
            _tokenService = tokenService;
        }

        public async Task<TokenDto> LoginAsync(string email, string password)
        {
            var user = await _userService.GetByEmailAsync(email);
            var verified = _hashService.Verify(password, user.Password);
            if (!verified)
            {
                throw new UnauthorizedAccessException();
            }

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user); ;

            return new TokenDto(accessToken, refreshToken); 
        }
    }
}
