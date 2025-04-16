namespace ShadowNetBackend.Dtos.Agents;

public record AgentForUpdateDto(
    Guid Id,
    string FirstName,
    string LastName,
    string? Alias,
    string? Specialization,
    ClearanceLevel ClearanceLevel,
    Guid? MissionId,
    string? Image);
