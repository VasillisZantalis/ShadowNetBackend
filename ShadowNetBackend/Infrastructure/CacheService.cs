using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ShadowNetBackend.Infrastructure.Interfaces;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetDataAsync<T>(string key)
    {
        var cachedData = await _cache.GetStringAsync(key);
        return cachedData is not null ? JsonSerializer.Deserialize<T>(cachedData) : default;
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        var serializedData = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(key, serializedData, options);
    }
}
