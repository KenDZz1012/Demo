using StackExchange.Redis;


namespace Account.Infrastructure.Services;

public class AccountRedisService
{
    private readonly IDatabase _redisDb;

    public AccountRedisService(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
    }
    
    public async Task<bool> IsUserOnlineAsync(string userId)
    {
        var value = await _redisDb.StringGetAsync($"online:{userId}");
        return value == "1";
    }
}