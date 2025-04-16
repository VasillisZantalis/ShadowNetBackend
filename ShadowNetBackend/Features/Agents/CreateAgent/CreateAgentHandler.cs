using Microsoft.AspNetCore.Identity;

namespace ShadowNetBackend.Features.Agents.CreateAgent;

public record CreateAgentCommand(AgentForCreationDto AgentForCreation) : ICommand<CreateAgentResult>;
public record CreateAgentResult(Guid Id);

internal class CreateAgentHandler(
    UserManager<Agent> userManager, 
    ApplicationDbContext dbContext,
    ICacheService cache) : ICommandHandler<CreateAgentCommand, CreateAgentResult>
{
    public async Task<CreateAgentResult> Handle(CreateAgentCommand command, CancellationToken cancellationToken)
    {
        var agent = command.AgentForCreation.ToAgent();

        var result = await userManager.CreateAsync(agent, command.AgentForCreation.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                   .GroupBy(e => e.Code)
                   .ToDictionary(
                       g => g.Key,
                       g => g.Select(e => e.Description).ToArray()
                   );
            throw new Exceptions.ValidationException(errors);
        }

        await userManager.AddToRoleAsync(agent, command.AgentForCreation.Rank.ToString());
        await dbContext.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(nameof(CacheKeys.Agents));

        return new CreateAgentResult(Guid.Parse(agent.Id));
    }
}
