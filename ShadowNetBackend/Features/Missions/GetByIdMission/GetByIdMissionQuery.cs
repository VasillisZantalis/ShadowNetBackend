using MediatR;
using ShadowNetBackend.Common;
using ShadowNetBackend.Features.Missions.Common;

namespace ShadowNetBackend.Features.Missions.GetByIdMission;

public record GetByIdMissionQuery(Guid Id, EncryptionType? DecryptionType) : IRequest<MissionResponse>;
