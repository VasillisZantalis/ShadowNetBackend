namespace ShadowNetBackend.Features.Auth.RefreshToken;

public class RefreshToken
{
    public string Token { get; set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset Expires { get; set; }
    public string UserId { get; set; } = string.Empty;
}
