namespace ShadowNetBackend.Features.Communications;

public class Message
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset? SentAt { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }
    public bool? IsDestroyed { get; set; }
}
