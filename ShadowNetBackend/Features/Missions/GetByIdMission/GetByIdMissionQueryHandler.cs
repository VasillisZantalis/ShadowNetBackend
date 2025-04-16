using ShadowNetBackend.Features.Missions.Common;

namespace ShadowNetBackend.Features.Missions.GetByIdMission;

public class GetByIdMissionQueryHandler : IRequestHandler<GetByIdMissionQuery, MissionResponse>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICryptographyService _cryptographyService;

    public GetByIdMissionQueryHandler(ApplicationDbContext dbContext, ICryptographyService cryptographyService)
    {
        _dbContext = dbContext;
        _cryptographyService = cryptographyService;
    }

    public async Task<MissionResponse> Handle(GetByIdMissionQuery request, CancellationToken cancellationToken)
    {
        var mission = await _dbContext.Missions
            .AsNoTracking()
            .Include(x => x.AssignedAgents)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (mission == null)
            throw new MissionNotFoundException();

        var encryptionType = request.EncryptionType ?? EncryptionType.None;

        var missionResponse = new MissionResponse
        {
            Id = mission.Id,
            Title = mission.Title,
            Image = mission.Image != null
                ? FileHelper.ConvertToBase64(mission.Image)
                : null,
            Objective = encryptionType != EncryptionType.None
                ? _cryptographyService.Decrypt(mission.Objective, encryptionType, request.EncryptionKey)
                : mission.Objective,
            Location = encryptionType != EncryptionType.None
                ? _cryptographyService.Decrypt(mission.Location, encryptionType, request.EncryptionKey)
                : mission.Location,
            Status = mission.Status,
            Risk = mission.Risk,
            Date = mission.Date,
            AssignedAgents = mission.AssignedAgents.Select(x => x.ToAgentDto()).ToList()
        };

        return missionResponse;
    }
}
