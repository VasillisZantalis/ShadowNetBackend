using MediatR;
using ShadowNetBackend.Exceptions;
using ShadowNetBackend.Extensions;
using ShadowNetBackend.Infrastructure.Data;

namespace ShadowNetBackend.Features.Missions.DeleteMission;

public class DeleteMissionCommandHandler : IRequestHandler<DeleteMissionCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;

    public DeleteMissionCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(DeleteMissionCommand request, CancellationToken cancellationToken)
    {
        if (!await _dbContext.ExistsAsync<Mission>(request.Id, cancellationToken))
            throw new NotFoundException();

        var mission = await _dbContext.Missions.FindAsync(request.Id);

        _dbContext.Missions.Remove(mission!);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
