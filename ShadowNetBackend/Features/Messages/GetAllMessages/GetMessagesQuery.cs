using ShadowNetBackend.Features.Messages.Common;

namespace ShadowNetBackend.Features.Messages.GetAllMessages;

public record GetMessagesQuery(MessageParameters Parameters) : IRequest<IEnumerable<MessageResponse>>;
