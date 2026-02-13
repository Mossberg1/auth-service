using Auth.Data;
using Auth.Dtos;
using Auth.Interfaces;
using Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHashService _hashService;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthService(ApplicationDbContext dbContext, IHashService hashService, IUserService userService, ITokenService tokenService) 
        {
            _dbContext = dbContext;
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
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user);

            return new TokenDto(accessToken, refreshToken); 
        }

        public async Task LogoutAsync(Guid userId)
        {
            await _dbContext.RefreshTokens
                .Where(rt => rt.UserId == userId)
                .ExecuteDeleteAsync();
        }

        public async Task<TokenDto> RefreshAccessToken(string refreshToken) 
        {
            var currentToken = await _dbContext.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (currentToken == null || currentToken.ExpiresAt < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException();
            }

            var newAccessToken = _tokenService.GenerateAccessToken(currentToken.User);
            var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync(currentToken.User);

            _dbContext.RefreshTokens.Remove(currentToken);
            await _dbContext.SaveChangesAsync();

            return new TokenDto(newAccessToken, newRefreshToken);
        }
    }
}
