using ShadowNetBackend.Common;

namespace ShadowNetBackend.Features.Missions;

public class Mission
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public byte[]? Image { get; set; }
    public string Objective { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public MissionStatus Status { get; set; }
    public RiskLevel Risk { get; set; }
    public DateOnly Date { get; set; }
    public ICollection<MissionAssignment> AssignedAgents { get; set; } = new List<MissionAssignment>();
}
