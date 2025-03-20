using ShadowNetBackend.Common.Helpers;
using ShadowNetBackend.Features.Agents;
using ShadowNetBackend.Features.Agents.Common;

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
            Image = agent.Image != null ? FileHelper.ConvertToBase64(agent.Image) : null,
            Alias = agent.Alias,
            Rank = agent.Rank,
            Specialization = agent.Specialization,
            ClearanceLevel = agent.ClearanceLevel
        };
    }
}
