using ShadowNetBackend.Features.Missions.Common;

namespace ShadowNetBackend.Features.Missions.DeleteMission;

public class DeleteMissionCommandHandler : IRequestHandler<DeleteMissionCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;

    public DeleteMissionCommandHandler(ApplicationDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteMissionCommand request, CancellationToken cancellationToken)
    {
        var mission = await _dbContext.Missions.FindAsync(request.Id);
        if (mission is null)
            throw new MissionNotFoundException();

        _dbContext.Missions.Remove(mission);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(nameof(CacheKeys.Missions));

        return true;
    }
}
