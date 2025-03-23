namespace ShadowNetBackend.Features.Agents.UpdateAgent;

public record UpdateAgentCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string? Alias,
    string? Specialization,
    ClearanceLevel ClearanceLevel,
    Guid? MissionId,
    string? Image) : IRequest<bool>;
