
using Auth.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Auth.BackgroundServices
{
    public class RefreshTokenCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private const int _idleHours = 1;

        public RefreshTokenCleanupService(IServiceScopeFactory serviceScopeFactory) 
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var res = await dbContext.RefreshTokens
                        .Where(t => t.ExpiresAt < DateTime.UtcNow)
                        .ExecuteDeleteAsync();
                }
                await Task.Delay(TimeSpan.FromHours(_idleHours), stoppingToken);
            }
        }
    }
}
