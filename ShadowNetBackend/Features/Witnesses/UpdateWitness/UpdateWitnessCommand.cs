namespace ShadowNetBackend.Features.Witnesses.UpdateWitness;

public record UpdateWitnessCommand(
    Guid Id,
    string? Alias,
    string? Image,
    RiskLevel RiskLevel,
    string? LocationHistory,
    RelocationStatus RelocationStatus) : IRequest<bool>;
