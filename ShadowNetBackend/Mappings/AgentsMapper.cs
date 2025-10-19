using ShadowNetBackend.Features.Agents.Common;

namespace ShadowNetBackend.Mappings;

public static class AgentsMapper
{
    public static AgentDto ToAgentDto(this Agent agent)
    {
        return new AgentDto
        (
            Guid.Parse(agent.Id),
            agent.FirstName,
            agent.LastName,
            agent.Image != null ? FileHelper.ConvertToBase64(agent.Image) : null,
            agent.Alias,
            agent.Rank,
            agent.Specialization,
            agent.ClearanceLevel
        );
    }

    public static IEnumerable<AgentDto> ToAgentDto(this IEnumerable<Agent> agents) => agents.Select(s => s.ToAgentDto());

    public static Agent ToAgent(this AgentForCreationDto agentForCreation)
    {
        return new Agent
        {
            UserName = agentForCreation.Email,
            Email = agentForCreation.Email,
            FirstName = agentForCreation.FirstName,
            LastName = agentForCreation.LastName,
            Image = agentForCreation.Image != null ? FileHelper.ConvertFromBase64(agentForCreation.Image) : null,
            Rank = agentForCreation.Rank,
            Alias = agentForCreation.Alias,
            Specialization = agentForCreation.Specialization,
            ClearanceLevel = agentForCreation.ClearanceLevel
        };
    }

    public static Agent ToAgent(this AgentForUpdateDto agentForUpdate)
    {
        return new Agent
        {
            Id = agentForUpdate.Id.ToString(),
            FirstName = agentForUpdate.FirstName,
            LastName = agentForUpdate.LastName,
            Image = agentForUpdate.Image != null ? FileHelper.ConvertFromBase64(agentForUpdate.Image) : null,
            Alias = agentForUpdate.Alias,
            Specialization = agentForUpdate.Specialization,
            ClearanceLevel = agentForUpdate.ClearanceLevel,
            MissionId = agentForUpdate.MissionId
        };
    }
}
