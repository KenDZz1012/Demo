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
            var key = $"presence:connections:{userId}";
            Console.WriteLine($"Setting user {userId} online with connection {connectionId}");
            await _db.SetAddAsync(key, connectionId);
            await _db.StringSetAsync($"presence:user:{userId}", "online", TimeSpan.FromSeconds(60));
        }

        public async Task SetUserOfflineAsync(string userId, string connectionId)
        {
            var key = $"presence:connections:{userId}";
            await _db.SetRemoveAsync(key, connectionId);
            var remaining = await _db.SetLengthAsync(key);
            if (remaining == 0)
            {
                await _db.StringSetAsync($"presence:user:{userId}", "offline");
                await _db.KeyDeleteAsync(key);
            }
        }

        public async Task<string?> GetUserStatusAsync(string userId)
        {
            return await _db.StringGetAsync($"presence:user:{userId}");
        }

        public async Task<Dictionary<string, bool>> GetBatchStatus(List<string> userIds)
        {

            RedisKey[] redisKeys = userIds
                .Select(id => (RedisKey)$"presence:user:{id}")
                .ToArray();

            RedisValue[] values = await _db.StringGetAsync(redisKeys);
            for (int i = 0; i < redisKeys.Length; i++)
            {
                Console.WriteLine($"[Redis] Key: {redisKeys[i]}, Value: {values[i]}");
            }
            return redisKeys.Zip(values, (key, val) => new
            {
                UserId = key.ToString().Substring("presence:user:".Length),
                IsOnline = val.HasValue && val == "online"
            }).ToDictionary(x => x.UserId, x => x.IsOnline);
        }
        
        public async Task<List<string>> GetConnectionIdsAsync(string userId)
        {
            var key = $"presence:connections:{userId}";
            var ids = await _db.SetMembersAsync(key);
            return ids.Select(id => (string)id).ToList();
        }
    }
}
