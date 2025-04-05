using ShadowNetBackend.Features.Agents.Common;
using ShadowNetBackend.Features.Missions.Common;

namespace ShadowNetBackend.Features.Agents.UpdateAgent;

public class UpdateAgentCommandHandler : IRequestHandler<UpdateAgentCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;

    public UpdateAgentCommandHandler(ApplicationDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<bool> Handle(UpdateAgentCommand request, CancellationToken cancellationToken)
    {
        if (request.MissionId.HasValue)
        {
            var exists = await _dbContext.ExistsAsync<Mission>(request.MissionId.Value, cancellationToken);
            if (!exists)
                throw new MissionNotFoundException();
        }

        var agent = await _dbContext.Agents.FirstAsync(a => a.Id == request.Id.ToString(), cancellationToken);
        if (agent is null)
            throw new AgentNotFoundException();

        agent.FirstName = request.FirstName;
        agent.LastName = request.LastName;
        agent.Alias = request.Alias;
        agent.Specialization = request.Specialization;
        agent.ClearanceLevel = request.ClearanceLevel;
        agent.MissionId = request.MissionId;
        agent.Image = request.Image is null ? null : FileHelper.ConvertFromBase64(request.Image);

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(nameof(CacheKeys.Agents));

        return true;
    }
}
