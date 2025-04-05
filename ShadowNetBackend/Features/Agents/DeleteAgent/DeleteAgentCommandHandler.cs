using ShadowNetBackend.Features.Agents.Common;

namespace ShadowNetBackend.Features.Agents.DeleteAgent;

public class DeleteAgentCommandHandler : IRequestHandler<DeleteAgentCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;

    public DeleteAgentCommandHandler(ApplicationDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteAgentCommand request, CancellationToken cancellationToken)
    {
        var agent = await _dbContext.Agents.FirstOrDefaultAsync(a => a.Id == request.Id.ToString(), cancellationToken);

        if (agent is null)
            throw new AgentNotFoundException();

        _dbContext.Agents.Remove(agent);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(nameof(CacheKeys.Agents));

        return true;
    }
}
