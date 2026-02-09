using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace Auth.Models
{
    [Index(nameof(Token), IsUnique = true)]
    [Index(nameof(ExpiresAt))]
    [Index(nameof(UserId))]
    public class RefreshToken : BaseModel
    {
        public string Token { get; init; }
        public DateTime ExpiresAt { get; init; } = DateTime.UtcNow.AddDays(7);
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }

        public RefreshToken() 
        {
            Token = GenerateRefreshToken();
        }


        public RefreshToken(User user) 
        {
            Token = GenerateRefreshToken();
            UserId = user.Id;
            User = user;
        }

        private string GenerateRefreshToken() 
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
