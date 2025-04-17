using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ShadowNetBackend.Infrastructure;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly RedisCacheSettings _settings;

    public CacheService(IDistributedCache cache, IOptions<RedisCacheSettings> cacheSettings)
    {
        _cache = cache;
        _settings = cacheSettings.Value;
    }

    public async Task<T?> GetDataAsync<T>(string key)
    {
        var cachedData = await _cache.GetStringAsync(key);
        if (cachedData is null) return default;

        try
        {
            return JsonSerializer.Deserialize<T>(cachedData);
        }
        catch (JsonException)
        {
            await _cache.RemoveAsync(key);
            return default;
        }
    }

    public async Task RemoveAsync(string key) => await _cache.RemoveAsync(key);

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        expiration ??= TimeSpan.FromSeconds(_settings.DefaultSlidingExpiration);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        var serializedData = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(key, serializedData, options);
    }
}
