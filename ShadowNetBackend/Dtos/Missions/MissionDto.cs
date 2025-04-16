namespace ShadowNetBackend.Dtos.Missions;

public record MissionDto(
    Guid Id,
    string Title,
    string? Image,
    string Objective,
    string Location,
    DateOnly Date,
    IEnumerable<AgentDto> AssignedAgents,
    MissionStatus Status = MissionStatus.Planned,
    RiskLevel Risk = RiskLevel.Low);