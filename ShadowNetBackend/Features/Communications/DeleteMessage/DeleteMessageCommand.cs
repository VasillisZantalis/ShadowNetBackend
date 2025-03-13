namespace ShadowNetBackend.Features.Communications.DeleteMessage;

public record DeleteMessageCommand(Guid Id) : IRequest<bool>;