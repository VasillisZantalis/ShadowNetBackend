using ShadowNetBackend.Features.Agents;

namespace ShadowNetBackend.Features.Missions;

public class MissionAssignment
{
    public Guid Id { get; set; }
    public string AgentId { get; set; } = string.Empty;
    public Agent? Agent { get; set; }
    public Guid MissionId { get; set; }
    public Mission? Mission { get; set; }
    public DateTimeOffset AssignedDate { get; set; }
}
