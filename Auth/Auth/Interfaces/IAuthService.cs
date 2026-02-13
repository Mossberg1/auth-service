using Auth.Dtos;

namespace Auth.Interfaces
{
    public interface IAuthService
    {
        Task<TokenDto> LoginAsync(string email, string password);
        Task LogoutAsync(Guid userId);
        Task<TokenDto> RefreshAccessToken(string refreshToken);
    }
}
