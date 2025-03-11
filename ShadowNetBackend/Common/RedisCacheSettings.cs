namespace ShadowNetBackend.Common;

public class RedisCacheSettings
{
    public bool Enabled { get; set; }
    public string ConnectionString { get; set; } = string.Empty;
    public int DefaultSlidingExpiration { get; set; }
}
