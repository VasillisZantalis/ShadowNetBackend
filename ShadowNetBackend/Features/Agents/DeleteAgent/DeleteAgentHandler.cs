using ShadowNetBackend.Features.Agents.Common;

namespace ShadowNetBackend.Features.Agents.DeleteAgent;

public record DeleteAgentCommand(Guid Id) : ICommand<bool>;

internal class DeleteAgentHandler(
    ApplicationDbContext dbContext,
    ICacheService cache) : ICommandHandler<DeleteAgentCommand, bool>
{
    public async Task<bool> Handle(DeleteAgentCommand request, CancellationToken cancellationToken)
    {
        var agent = await dbContext.Agents.FindAsync([request.Id.ToString()], cancellationToken);

        if (agent is null)
            throw new AgentNotFoundException();

        dbContext.Agents.Remove(agent);
        await dbContext.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(nameof(CacheKeys.Agents));

        return true;
    }
}
