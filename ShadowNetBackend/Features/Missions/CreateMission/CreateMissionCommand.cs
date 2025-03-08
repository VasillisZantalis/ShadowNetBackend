using MediatR;
using ShadowNetBackend.Common;

namespace ShadowNetBackend.Features.Missions.CreateMission;

public class CreateMissionCommand : IRequest<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string? Image { get; set; }
    public string Objective { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public MissionStatus Status { get; set; } = MissionStatus.Planned;
    public RiskLevel Risk { get; set; } = RiskLevel.Low;
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public EncryptionType EncryptionType { get; set; } = EncryptionType.None;
    public string? EncryptionKey { get; set; }
}
