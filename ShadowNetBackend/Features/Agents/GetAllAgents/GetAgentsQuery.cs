using MediatR;

namespace ShadowNetBackend.Features.Agents.GetAllAgents;

public record GetAgentsQuery(AgentParameters Parameters) : IRequest<IEnumerable<AgentResponse>>;
