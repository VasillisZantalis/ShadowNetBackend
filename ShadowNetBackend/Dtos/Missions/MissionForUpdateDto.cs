namespace ShadowNetBackend.Dtos.Missions;

public record MissionForUpdateDto(
    Guid Id,
    string? Image,
    MissionStatus Status,
    RiskLevel Risk);
