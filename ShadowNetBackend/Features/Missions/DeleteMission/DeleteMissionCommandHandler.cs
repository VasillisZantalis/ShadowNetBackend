using ShadowNetBackend.Features.Missions.GetByIdMission;

namespace ShadowNetBackend.Features.Missions.DeleteMission;

public class DeleteMissionCommandHandler : IRequestHandler<DeleteMissionCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISender _sender;
    private readonly ICacheService _cache;

    public DeleteMissionCommandHandler(ApplicationDbContext dbContext, ISender sender, ICacheService cache)
    {
        _dbContext = dbContext;
        _sender = sender;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteMissionCommand request, CancellationToken cancellationToken)
    {
        await _sender.Send(new GetByIdMissionQuery(request.Id, null, null), cancellationToken);

        var mission = await _dbContext.Missions.FindAsync(request.Id);

        _dbContext.Missions.Remove(mission!);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(nameof(CacheKeys.Missions));

        return true;
    }
}
