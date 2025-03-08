using MediatR;
using ShadowNetBackend.Features.Missions.GetByIdMission;
using ShadowNetBackend.Infrastructure.Data;

namespace ShadowNetBackend.Features.Missions.DeleteMission;

public class DeleteMissionCommandHandler : IRequestHandler<DeleteMissionCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISender _sender;

    public DeleteMissionCommandHandler(ApplicationDbContext dbContext, ISender sender)
    {
        _dbContext = dbContext;
        _sender = sender;
    }

    public async Task<bool> Handle(DeleteMissionCommand request, CancellationToken cancellationToken)
    {
        await _sender.Send(new GetByIdMissionQuery(request.Id, null, null), cancellationToken);

        var mission = await _dbContext.Missions.FindAsync(request.Id);

        _dbContext.Missions.Remove(mission!);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
