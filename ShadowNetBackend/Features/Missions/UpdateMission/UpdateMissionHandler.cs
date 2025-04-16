using ShadowNetBackend.Features.Missions.Common;

namespace ShadowNetBackend.Features.Missions.UpdateMission;

public record UpdateMissionCommand(MissionForUpdateDto MissionForUpdate) : ICommand<bool>;

public class UpdateMissionCommandValidator : AbstractValidator<UpdateMissionCommand>
{
    public UpdateMissionCommandValidator()
    {
        RuleFor(x => x.MissionForUpdate.Status).NotEmpty();
        RuleFor(x => x.MissionForUpdate.Risk).NotEmpty();
    }
}
internal class UpdateMissionHandler(
    ApplicationDbContext dbContext,
    ICacheService cache) : ICommandHandler<UpdateMissionCommand, bool>
{
    public async Task<bool> Handle(UpdateMissionCommand request, CancellationToken cancellationToken)
    {
        var mission = await dbContext.Missions.FirstOrDefaultAsync(x => x.Id == request.MissionForUpdate.Id, cancellationToken);
        if (mission is null)
            throw new MissionNotFoundException();

        mission!.Status = request.MissionForUpdate.Status;
        mission.Risk = request.MissionForUpdate.Risk;
        mission.Image = request.MissionForUpdate.Image != null
            ? FileHelper.ConvertFromBase64(request.MissionForUpdate.Image)
            : null;

        dbContext.Missions.Update(mission);
        await dbContext.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(nameof(CacheKeys.Missions));
        return true;
    }
}
