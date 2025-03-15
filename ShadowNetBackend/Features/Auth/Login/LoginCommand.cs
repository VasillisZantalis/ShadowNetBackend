namespace ShadowNetBackend.Features.Auth.Login;

public record LoginCommand(string Email, string Password) : IRequest<object?>;
