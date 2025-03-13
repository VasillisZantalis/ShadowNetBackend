namespace ShadowNetBackend.Features.Messages.DeleteMessage;

public record DeleteMessageCommand(Guid Id) : IRequest<bool>;