namespace ShadowNetBackend.Features.Criminals.DeleteCriminal;

public record DeleteCriminalCommand(Guid Id) : IRequest<bool>;
