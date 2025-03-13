namespace ShadowNetBackend.Features.Agents.DeleteAgent;

public record DeleteAgentCommand(Guid Id) : IRequest<bool>;
