namespace ShadowNetBackend.Dtos.Missions;

public class MissionForCreationDto
{
    public string Title { get; set; } = string.Empty;
    public string? Image { get; set; }
    public string Objective { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string? EncryptionKey { get; set; }
    public MissionStatus Status { get; set; } = MissionStatus.Planned;
    public RiskLevel Risk { get; set; } = RiskLevel.Low;
    public DateOnly Date { get; set; } = default;
    public EncryptionType EncryptionType { get; set; } = EncryptionType.None;
}

