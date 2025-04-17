namespace ShadowNetBackend.Dtos.Witnesses;

public record WitnessForUpdateDto(
    Guid Id,
    string? Alias,
    string? Image,
    RiskLevel RiskLevel,
    string? LocationHistory,
    RelocationStatus RelocationStatus);