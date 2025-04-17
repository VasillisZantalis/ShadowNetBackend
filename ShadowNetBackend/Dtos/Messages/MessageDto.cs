namespace ShadowNetBackend.Dtos.Messages;

public record MessageDto(
    Guid Id,
    Guid SenderId,
    Guid ReceiverId,
    string Title,
    string Content,
    DateTimeOffset? SentAt);
