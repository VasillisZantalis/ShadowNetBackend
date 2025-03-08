using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Exceptions;
using ShadowNetBackend.Extensions;
using ShadowNetBackend.Helpers;
using ShadowNetBackend.Infrastructure.Data;
using static System.Net.Mime.MediaTypeNames;

namespace ShadowNetBackend.Features.Missions.UpdateMission;

public class UpdateMissionCommandHandler : IRequestHandler<UpdateMissionCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;

    public UpdateMissionCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(UpdateMissionCommand request, CancellationToken cancellationToken)
    {
        if (!await _dbContext.ExistsAsync<Mission>(request.Id, cancellationToken))
            throw new NotFoundException();

        var mission = await _dbContext.Missions.FindAsync(request.Id, cancellationToken);

        mission.Status = request.Status;
        mission.Risk = request.Risk;
        mission.Image = request.Image != null
            ? FileHelper.ConvertFromBase64(request.Image)
            : null;

        _dbContext.Missions.Update(mission);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
