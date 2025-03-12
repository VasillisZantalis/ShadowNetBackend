namespace ShadowNetBackend.Infrastructure.Interfaces;

public interface ICacheService
{
    Task<T?> GetDataAsync<T>(string key);
    Task RemoveAsync(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
}
