using ShadowNetBackend.Features.Missions.Common;

namespace ShadowNetBackend.Features.Missions.GetAllMissions;

public record GetMissionsQuery(MissionParameters Parameters) : IRequest<IEnumerable<MissionResponse>>;
