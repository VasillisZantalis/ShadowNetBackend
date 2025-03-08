using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Features.Missions.GetByIdMission;
using ShadowNetBackend.Helpers;
using ShadowNetBackend.Infrastructure.Data;

namespace ShadowNetBackend.Features.Missions.UpdateMission;

public class UpdateMissionCommandHandler : IRequestHandler<UpdateMissionCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISender _sender;

    public UpdateMissionCommandHandler(ApplicationDbContext dbContext, ISender sender)
    {
        _dbContext = dbContext;
        _sender = sender;
    }

    public async Task<bool> Handle(UpdateMissionCommand request, CancellationToken cancellationToken)
    {
        await _sender.Send(new GetByIdMissionQuery(request.Id, null, null), cancellationToken);

        var mission = await _dbContext.Missions.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        mission!.Status = request.Status;
        mission.Risk = request.Risk;
        mission.Image = request.Image != null
            ? FileHelper.ConvertFromBase64(request.Image)
            : null;

        _dbContext.Missions.Update(mission);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
