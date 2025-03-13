namespace ShadowNetBackend.Features.Communications.CreateMessage;

public record CreateMessageCommand(
    Guid SenderId,
    Guid ReceiverId,
    string Title,
    string Content,
    DateTimeOffset? SentAt,
    DateTimeOffset? ExpiresAt,
    bool? IsDestroyed,
    TimeSpan? AutoDestructTime) : IRequest<Guid>;
