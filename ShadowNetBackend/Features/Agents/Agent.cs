using Microsoft.AspNetCore.Identity;

namespace ShadowNetBackend.Features.Agents;

public class Agent : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public byte[]? Image { get; set; }
    public string? Alias { get; set; }
    public UserRank Rank { get; set; }
    public string? Specialization { get; set; }
    public ClearanceLevel ClearanceLevel { get; set; }
    public Guid? MissionId { get; set; }
    public Mission? Mission { get; set; }
}