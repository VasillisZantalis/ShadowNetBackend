using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Common;
using ShadowNetBackend.Common.Helpers;
using ShadowNetBackend.Features.Missions.Common;
using ShadowNetBackend.Features.Missions.GetByIdMission;
using ShadowNetBackend.Infrastructure.Data;
using ShadowNetBackend.Infrastructure.Interfaces;

namespace ShadowNetBackend.Features.Missions.UpdateMission;

public class UpdateMissionCommandHandler : IRequestHandler<UpdateMissionCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;

    public UpdateMissionCommandHandler(ApplicationDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<bool> Handle(UpdateMissionCommand request, CancellationToken cancellationToken)
    {
        var mission = await _dbContext.Missions.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (mission is null)
            throw new MissionNotFoundException();

        mission!.Status = request.Status;
        mission.Risk = request.Risk;
        mission.Image = request.Image != null
            ? FileHelper.ConvertFromBase64(request.Image)
            : null;

        _dbContext.Missions.Update(mission);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(nameof(CacheKeys.Missions));

        return true;
    }
}
