using ShadowNetBackend.Features.Agents.Common;

namespace ShadowNetBackend.Features.Agents.GetByIdAgent;

public record GetByIdAgentQuery(Guid Id) : IRequest<AgentResponse>;
