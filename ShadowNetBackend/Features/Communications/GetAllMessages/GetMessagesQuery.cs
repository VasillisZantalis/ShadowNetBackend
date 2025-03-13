namespace ShadowNetBackend.Features.Communications.GetAllMessages;

public record GetMessagesQuery(MessageParameters Parameters) : IRequest<IEnumerable<MessageResponse>>;
