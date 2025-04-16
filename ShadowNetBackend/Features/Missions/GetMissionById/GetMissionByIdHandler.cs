using ShadowNetBackend.Features.Missions.Common;

namespace ShadowNetBackend.Features.Missions.GetByIdMission;

public record GetByIdMissionQuery(Guid Id, EncryptionType? EncryptionType, string? EncryptionKey) : IQuery<MissionDto>;

internal class GetMissionByIdHandler(
    ApplicationDbContext dbContext,
    ICryptographyService cryptographyService) : IQueryHandler<GetByIdMissionQuery, MissionDto>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly ICryptographyService _cryptographyService = cryptographyService;

    public async Task<MissionDto> Handle(GetByIdMissionQuery request, CancellationToken cancellationToken)
    {
        var mission = await _dbContext.Missions
            .AsNoTracking()
            .Include(x => x.AssignedAgents)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (mission == null)
            throw new MissionNotFoundException();

        var encryptionType = request.EncryptionType ?? EncryptionType.None;

        var missionDto = new MissionDto
        (
            mission.Id,
            mission.Title,
            mission.Image != null
                ? FileHelper.ConvertToBase64(mission.Image)
                : null,
            encryptionType != EncryptionType.None
                ? _cryptographyService.Decrypt(mission.Objective, encryptionType, request.EncryptionKey)
                : mission.Objective,
            encryptionType != EncryptionType.None
                ? _cryptographyService.Decrypt(mission.Location, encryptionType, request.EncryptionKey)
                : mission.Location,
            mission.Date,
            mission.AssignedAgents.ToAgentDto(),
            mission.Status,
            mission.Risk
        );

        return missionDto;
    }
}
