namespace ShadowNetBackend.Features.Criminals.CreateCriminal;

public record CreateCriminalCommand(CriminalForCreationDto CriminalForCreation) : ICommand<Guid>;

public class CreateCriminalCommandValidator : AbstractValidator<CreateCriminalCommand>
{
    public CreateCriminalCommandValidator()
    {
        RuleFor(x => x.CriminalForCreation.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(250).WithMessage("First name must not exceed 250 characters");

        RuleFor(x => x.CriminalForCreation.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(250).WithMessage("Last name must not exceed 250 characters");

        RuleFor(x => x.CriminalForCreation.ThreatLevel)
            .IsInEnum().WithMessage("Threat level is invalid");

        RuleFor(x => x.CriminalForCreation.Alias)
            .MaximumLength(250);

        RuleFor(x => x.CriminalForCreation.Nationality)
            .MaximumLength(250);

        RuleFor(x => x.CriminalForCreation.LastKnownLocation)
            .MaximumLength(250).WithMessage("Last Known Location must not exceed 250 characters");
    }
}


internal class CreateCriminalHandler(
    ApplicationDbContext dbContext,
    ICacheService cache) : ICommandHandler<CreateCriminalCommand, Guid>
{
    public async Task<Guid> Handle(CreateCriminalCommand request, CancellationToken cancellationToken)
    {
        var criminal = request.CriminalForCreation.ToCriminal();

        dbContext.Criminals.Add(criminal);
        await dbContext.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(nameof(CacheKeys.Criminals));

        return criminal.Id;
    }
}
