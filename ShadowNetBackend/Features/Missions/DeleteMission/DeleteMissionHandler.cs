using ShadowNetBackend.Features.Missions.Common;

namespace ShadowNetBackend.Features.Missions.DeleteMission;

public record DeleteMissionCommand(Guid Id) : ICommand<bool>;

internal class DeleteMissionHandler(
    ApplicationDbContext dbContext,
    ICacheService cache) : ICommandHandler<DeleteMissionCommand, bool>
{
    public async Task<bool> Handle(DeleteMissionCommand request, CancellationToken cancellationToken)
    {
        var mission = await dbContext.Missions.FindAsync(request.Id);
        if (mission is null)
            throw new MissionNotFoundException();

        dbContext.Missions.Remove(mission);
        await dbContext.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(nameof(CacheKeys.Missions));
        return true;
    }
}
