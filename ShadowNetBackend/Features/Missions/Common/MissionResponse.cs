using ShadowNetBackend.Common;

namespace ShadowNetBackend.Features.Missions.Common;

public class MissionResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Image { get; set; }
    public string Objective { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public MissionStatus Status { get; set; } = MissionStatus.Planned;
    public RiskLevel Risk { get; set; } = RiskLevel.Low;
    public DateOnly Date { get; set; }
}
