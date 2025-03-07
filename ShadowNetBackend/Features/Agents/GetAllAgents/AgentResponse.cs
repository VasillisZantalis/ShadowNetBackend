using ShadowNetBackend.Common;

namespace ShadowNetBackend.Features.Agents.GetAllAgents;

public class AgentResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Alias { get; set; }
    public UserRank Rank { get; set; }
    public string? Specialization { get; set; }
    public ClearanceLevel ClearanceLevel { get; set; }
}
