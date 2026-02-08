using Auth.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace Auth.Services
{
    public class HashService : IHashService
    {
        public string Hash(string str)
        {
            return BC.EnhancedHashPassword(str, 13);
        }

        public bool Verify(string str, string hash)
        {
            return BC.EnhancedVerify(str, hash);
        }
    }
}
