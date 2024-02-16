using AppyNox.Services.Base.Application.Interfaces.Caches;
using StackExchange.Redis;

namespace AppyNox.Services.Base.Infrastructure.Services.CacheServices;

public class RedisCacheService : ICacheService
{
    #region [ Fields ]

    private readonly IConnectionMultiplexer _redis;

    #endregion

    #region [ Public Constructors ]

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    #endregion

    #region [ Public Methods ]

    public virtual async Task<string?> GetCachedValueAsync(string key)
    {
        var db = _redis.GetDatabase();
        return await db.StringGetAsync(key);
    }

    public virtual async Task SetCachedValueAsync(string key, string value, TimeSpan? expiry = null)
    {
        var db = _redis.GetDatabase();
        await db.StringSetAsync(key, value, expiry);
    }

    #endregion
}