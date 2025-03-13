namespace ShadowNetBackend.Features.Communications.SendMessage;

public record SendMessageCommand(string User, string Message) : IRequest<Unit>;

