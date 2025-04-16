namespace ShadowNetBackend.Dtos.Missions;

public record MissionForCreationDto(
    string Title,
    string? Image,
    string Objective,
    string Location,
    string? EncryptionKey,
    MissionStatus Status = MissionStatus.Planned,
    RiskLevel Risk = RiskLevel.Low,
    DateOnly Date = default,
    EncryptionType EncryptionType = EncryptionType.None);
