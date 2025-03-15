namespace ShadowNetBackend.Features.Auth.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<string?>;