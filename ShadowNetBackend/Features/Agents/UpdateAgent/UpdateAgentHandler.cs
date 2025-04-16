using ShadowNetBackend.Features.Agents.Common;
using ShadowNetBackend.Features.Missions.Common;

namespace ShadowNetBackend.Features.Agents.UpdateAgent;

public record UpdateAgentCommand(AgentForUpdateDto AgentForUpdate) : ICommand<bool>;

internal class UpdateAgentHandler(
    ApplicationDbContext dbContext,
    ICacheService cache) : ICommandHandler<UpdateAgentCommand, bool>
{
    public async Task<bool> Handle(UpdateAgentCommand command, CancellationToken cancellationToken)
    {
        var agent = await dbContext.Agents.FindAsync([command.AgentForUpdate.Id.ToString()], cancellationToken);
        if (agent is null)
            throw new AgentNotFoundException();

        if (command.AgentForUpdate.MissionId.HasValue)
        {
            var exists = await dbContext.ExistsAsync<Mission>(command.AgentForUpdate.MissionId.Value, cancellationToken);
            if (!exists)
                throw new MissionNotFoundException();
        }

        agent.FirstName = command.AgentForUpdate.FirstName;
        agent.LastName = command.AgentForUpdate.LastName;
        agent.Alias = command.AgentForUpdate.Alias;
        agent.Specialization = command.AgentForUpdate.Specialization;
        agent.ClearanceLevel = command.AgentForUpdate.ClearanceLevel;
        agent.MissionId = command.AgentForUpdate.MissionId;
        agent.Image = command.AgentForUpdate.Image is null ? null : FileHelper.ConvertFromBase64(command.AgentForUpdate.Image);

        await dbContext.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(nameof(CacheKeys.Agents));

        return true;
    }
}
