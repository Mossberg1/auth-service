using Auth.Dtos;

namespace Auth.Interfaces
{
    public interface IAuthService
    {
        Task<TokenDto> LoginAsync(string email, string password);
    }
}
