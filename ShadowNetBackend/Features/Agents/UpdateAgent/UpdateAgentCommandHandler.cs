using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Common;
using ShadowNetBackend.Features.Agents.GetByIdAgent;
using ShadowNetBackend.Features.Missions.GetByIdMission;
using ShadowNetBackend.Infrastructure.Data;
using ShadowNetBackend.Infrastructure.Interfaces;

namespace ShadowNetBackend.Features.Agents.UpdateAgent;

public class UpdateAgentCommandHandler : IRequestHandler<UpdateAgentCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISender _sender;
    private readonly ICacheService _cache;

    public UpdateAgentCommandHandler(ApplicationDbContext dbContext, ISender sender, ICacheService cache)
    {
        _dbContext = dbContext;
        _sender = sender;
        _cache = cache;
    }

    public async Task<bool> Handle(UpdateAgentCommand request, CancellationToken cancellationToken)
    {
        await _sender.Send(new GetByIdAgentQuery(request.Id), cancellationToken);

        if (request.MissionId.HasValue)
            await _sender.Send(new GetByIdMissionQuery(request.MissionId.Value, null, null), cancellationToken);

        var agent = await _dbContext.Agents.FirstAsync(a => a.Id == request.Id.ToString(), cancellationToken);

        agent.FirstName = request.FirstName;
        agent.LastName = request.LastName;
        agent.Alias = request.Alias;
        agent.Specialization = request.Specialization;
        agent.ClearanceLevel = request.ClearanceLevel;
        agent.MissionId = request.MissionId;

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync($"{CacheKeys.Agent}_{request.Id}");

        return true;
    }
}
