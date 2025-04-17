using ShadowNetBackend.Dtos.Witnesses;

namespace ShadowNetBackend.Features.Witnesses.CreateWitness;

public record CreateWitnessCommand(WitnessForCreationDto WitnessForCreation) : ICommand<Guid>;

public class CreateWitnessValidator : AbstractValidator<CreateWitnessCommand>
{
    public CreateWitnessValidator()
    {
        RuleFor(x => x.WitnessForCreation.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(250).WithMessage("First name must not exceed 250 characters");

        RuleFor(x => x.WitnessForCreation.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(250).WithMessage("Last name must not exceed 250 characters");

        RuleFor(x => x.WitnessForCreation.Alias).NotEmpty();
        RuleFor(x => x.WitnessForCreation.RiskLevel).NotEmpty().WithMessage("Risk Level is required");
    }
}

internal class CreateWitnessHandler(
    ApplicationDbContext dbContext,
    ICacheService cache) : ICommandHandler<CreateWitnessCommand, Guid>
{
    public async Task<Guid> Handle(CreateWitnessCommand request, CancellationToken cancellationToken)
    {
        var witness = request.WitnessForCreation.ToWitness();

        dbContext.Witnesses.Add(witness);
        await dbContext.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(nameof(CacheKeys.Witnesses));
        return witness.Id;
    }
}