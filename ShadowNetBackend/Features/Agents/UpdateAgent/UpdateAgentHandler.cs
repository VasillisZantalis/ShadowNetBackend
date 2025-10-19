using ShadowNetBackend.Features.Agents.Common;
using ShadowNetBackend.Features.Missions.Common;

namespace ShadowNetBackend.Features.Agents.UpdateAgent;

public record UpdateAgentCommand(AgentForUpdateDto AgentForUpdate) : ICommand<bool>;

public class UpdateAgentCommandValidator : AbstractValidator<UpdateAgentCommand>
{
    public UpdateAgentCommandValidator()
    {
        RuleFor(x => x.AgentForUpdate.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(250).WithMessage("First name must not exceed 250 characters");

        RuleFor(x => x.AgentForUpdate.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(250).WithMessage("Last name must not exceed 250 characters");
    }
}

internal class UpdateAgentHandler(
    ApplicationDbContext dbContext,
    ICacheService cache) : ICommandHandler<UpdateAgentCommand, bool>
{
    public async Task<bool> Handle(UpdateAgentCommand command, CancellationToken cancellationToken)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
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

            dbContext.Entry(agent).CurrentValues.SetValues(command.AgentForUpdate.ToAgent());
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            await cache.RemoveAsync(nameof(CacheKeys.Agents));

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
