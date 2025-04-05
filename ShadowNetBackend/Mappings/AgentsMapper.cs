using ShadowNetBackend.Features.Agents.Common;
using ShadowNetBackend.Features.Agents.CreateAgent;

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

    public static Agent ToAgent(this CreateAgentCommand command)
    {
        return new Agent
        {
            UserName = command.Email,
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Image = command.Image != null ? FileHelper.ConvertFromBase64(command.Image) : null,
            Rank = command.Rank,
            Alias = command.Alias,
            Specialization = command.Specialization,
            ClearanceLevel = command.ClearanceLevel
        };
    }
}
