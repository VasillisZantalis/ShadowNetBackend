using ShadowNetBackend.Features.Agents.GetAllAgents;
using ShadowNetBackend.Features.Agents;

namespace ShadowNetBackend.Mappings;

public static class AgentsMapper
{
    public static AgentResponse ToAgentResponse(this Agent agent)
    {
        return new AgentResponse
        {
            Id = Guid.Parse(agent.Id),
            FirstName = agent.FirstName,
            LastName = agent.LastName,
            Alias = agent.Alias,
            Rank = agent.Rank,
            Specialization = agent.Specialization,
            ClearanceLevel = agent.ClearanceLevel
        };
    }
}
