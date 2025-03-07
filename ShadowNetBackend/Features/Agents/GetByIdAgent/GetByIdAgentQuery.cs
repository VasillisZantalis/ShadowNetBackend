using MediatR;
using ShadowNetBackend.Features.Agents.GetAllAgents;

namespace ShadowNetBackend.Features.Agents.GetByIdAgent;

public record GetByIdAgentQuery(Guid Id) : IRequest<AgentResponse>;
