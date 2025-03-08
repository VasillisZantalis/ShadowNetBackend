using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Common;
using ShadowNetBackend.Exceptions;
using ShadowNetBackend.Features.Missions.Common;
using ShadowNetBackend.Helpers;
using ShadowNetBackend.Infrastructure.Data;
using ShadowNetBackend.Infrastructure.Security;

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
        var mission = await _dbContext.Missions.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (mission == null)
            throw new NotFoundException();

        var encryptionType = request.DecryptionType ?? EncryptionType.None;

        return new MissionResponse
        {
            Id = mission.Id,
            Title = mission.Title,
            Image = mission.Image != null
                ? FileHelper.ConvertToBase64(mission.Image)
                : null,
            Objective = encryptionType != EncryptionType.None
                ? _cryptographyService.Decrypt(mission.Objective, encryptionType)
                : mission.Objective,
            Location = encryptionType != EncryptionType.None
                ? _cryptographyService.Decrypt(mission.Location, encryptionType)
                : mission.Location,
            Status = mission.Status,
            Risk = mission.Risk,
            Date = mission.Date
        };
    }
}
