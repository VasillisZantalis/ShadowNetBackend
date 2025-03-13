namespace ShadowNetBackend.Features.Communications.Common;

public record MessageResponse(
    Guid Id,
    Guid SenderId,
    Guid ReceiverId,
    string Title,
    string Content,
    DateTimeOffset? SentAt);
