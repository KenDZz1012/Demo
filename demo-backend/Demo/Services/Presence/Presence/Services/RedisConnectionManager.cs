using StackExchange.Redis;

namespace Presence.Services
{
    public class RedisConnectionManager : IConnectionManager
    {
        private readonly IDatabase _db;

        public RedisConnectionManager(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task SetUserOnlineAsync(string userId, string connectionId)
        {
            Console.WriteLine($"Setting user {userId} online with connection {connectionId}");
            await _db.StringSetAsync($"presence:user:{userId}", "online", TimeSpan.FromSeconds(60));
        }

        public async Task SetUserOfflineAsync(string userId)
        {
            await _db.StringSetAsync($"presence:user:{userId}", "offline");
        }

        public async Task<string?> GetUserStatusAsync(string userId)
        {
            return await _db.StringGetAsync($"presence:user:{userId}");
        }
    }
}
