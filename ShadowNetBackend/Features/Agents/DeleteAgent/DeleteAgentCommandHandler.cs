using ShadowNetBackend.Features.Agents.GetByIdAgent;

namespace ShadowNetBackend.Features.Agents.DeleteAgent;

public class DeleteAgentCommandHandler : IRequestHandler<DeleteAgentCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISender _sender;
    private readonly ICacheService _cache;

    public DeleteAgentCommandHandler(ApplicationDbContext dbContext, ISender sender, ICacheService cache)
    {
        _dbContext = dbContext;
        _sender = sender;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteAgentCommand request, CancellationToken cancellationToken)
    {
        await _sender.Send(new GetByIdAgentQuery(request.Id), cancellationToken);

        var agent = await _dbContext.Agents.FirstOrDefaultAsync(a => a.Id == request.Id.ToString(), cancellationToken);

        _dbContext.Agents.Remove(agent!);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(nameof(CacheKeys.Agents));

        return true;
    }
}
