using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Common;
using ShadowNetBackend.Features.Missions.GetByIdMission;
using ShadowNetBackend.Helpers;
using ShadowNetBackend.Infrastructure.Data;
using ShadowNetBackend.Infrastructure.Interfaces;

namespace ShadowNetBackend.Features.Missions.UpdateMission;

public class UpdateMissionCommandHandler : IRequestHandler<UpdateMissionCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISender _sender;
    private readonly ICacheService _cache;

    public UpdateMissionCommandHandler(ApplicationDbContext dbContext, ISender sender, ICacheService cache)
    {
        _dbContext = dbContext;
        _sender = sender;
        _cache = cache;
    }

    public async Task<bool> Handle(UpdateMissionCommand request, CancellationToken cancellationToken)
    {
        string cacheKey = $"{CacheKeys.Mission}_{request.Id}";

        await _sender.Send(new GetByIdMissionQuery(request.Id, null, null), cancellationToken);

        var mission = await _dbContext.Missions.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        mission!.Status = request.Status;
        mission.Risk = request.Risk;
        mission.Image = request.Image != null
            ? FileHelper.ConvertFromBase64(request.Image)
            : null;

        _dbContext.Missions.Update(mission);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(cacheKey);

        return true;
    }
}
