using Microsoft.AspNetCore.Identity;

namespace ShadowNetBackend.Features.Agents.CreateAgent;

public record CreateAgentCommand(AgentForCreationDto AgentForCreation) : ICommand<CreateAgentResult>;
public record CreateAgentResult(Guid Id);

public class CreateAgentCommandValidator : AbstractValidator<CreateAgentCommand>
{
    public CreateAgentCommandValidator()
    {
        RuleFor(x => x.AgentForCreation.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(250).WithMessage("First name must not exceed 250 characters");

        RuleFor(x => x.AgentForCreation.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(250).WithMessage("Last name must not exceed 250 characters");

        RuleFor(x => x.AgentForCreation.ClearanceLevel)
            .NotNull().WithMessage("Clearance Level is required");
    }
}


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
            throw new Exceptions.ValidationFailedException(errors);
        }

        await userManager.AddToRoleAsync(agent, command.AgentForCreation.Rank.ToString());
        await dbContext.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(nameof(CacheKeys.Agents));

        return new CreateAgentResult(Guid.Parse(agent.Id));
    }
}
