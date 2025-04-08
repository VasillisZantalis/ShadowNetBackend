namespace ShadowNetBackend.Features.Messages.GetAllMessages;

public record GetMessagesQuery(MessageParameters Parameters) : IRequest<IEnumerable<MessageResponse>>;
