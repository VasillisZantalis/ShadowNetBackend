using MediatR;
using ShadowNetBackend.Common;

namespace ShadowNetBackend.Features.Agents.CreateAgent;

public class CreateAgentCommand : IRequest<Guid?>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Image { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Alias { get; set; }
    public UserRank Rank { get; set; } = UserRank.Agent;
    public string? Specialization { get; set; }
    public ClearanceLevel ClearanceLevel { get; set; } = ClearanceLevel.None;
}
