namespace ShadowNetBackend.Dtos.Messages;

public record MessageForCreationDto(
    Guid SenderId,
    Guid ReceiverId,
    string Title,
    string Content,
    DateTimeOffset? SentAt,
    DateTimeOffset? ExpiresAt,
    bool? IsDestroyed,
    TimeSpan? AutoDestructTime);
