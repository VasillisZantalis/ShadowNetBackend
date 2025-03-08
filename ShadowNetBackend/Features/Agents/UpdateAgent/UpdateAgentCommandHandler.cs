using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Exceptions;
using ShadowNetBackend.Extensions;
using ShadowNetBackend.Features.Agents.GetByIdAgent;
using ShadowNetBackend.Features.Missions.GetByIdMission;
using ShadowNetBackend.Infrastructure.Data;

namespace ShadowNetBackend.Features.Agents.UpdateAgent;

public class UpdateAgentCommandHandler : IRequestHandler<UpdateAgentCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISender _sender;

    public UpdateAgentCommandHandler(ApplicationDbContext dbContext, ISender sender)
    {
        _dbContext = dbContext;
        _sender = sender;
    }

    public async Task<bool> Handle(UpdateAgentCommand request, CancellationToken cancellationToken)
    {
        await _sender.Send(new GetByIdAgentQuery(request.Id), cancellationToken);

        if (request.MissionId.HasValue)
            await _sender.Send(new GetByIdMissionQuery(request.MissionId.Value, null), cancellationToken);

        var agent = await _dbContext.Agents.FirstAsync(a => a.Id == request.Id.ToString(), cancellationToken);

        agent.FirstName = request.FirstName;
        agent.LastName = request.LastName;
        agent.Alias = request.Alias;
        agent.Specialization = request.Specialization;
        agent.ClearanceLevel = request.ClearanceLevel;
        agent.MissionId = request.MissionId;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
