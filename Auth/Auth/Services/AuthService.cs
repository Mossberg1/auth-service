using Auth.Interfaces;

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

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userService.GetByEmailAsync(email);
            var verified = _hashService.Verify(password, user.Password);
            if (!verified)
            {
                throw new UnauthorizedAccessException();
            }

            return _tokenService.Generate(user);
        }
    }
}
