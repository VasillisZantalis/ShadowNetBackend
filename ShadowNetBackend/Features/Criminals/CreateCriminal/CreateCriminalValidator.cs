namespace ShadowNetBackend.Features.Criminals.CreateCriminal;

public class CreateCriminalValidator : AbstractValidator<CreateCriminalCommand>
{
    public CreateCriminalValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(250).WithMessage("First name must not exceed 250 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(250).WithMessage("Last name must not exceed 250 characters");

        RuleFor(x => x.ThreatLevel)
            .IsInEnum().WithMessage("Threat level is invalid");

        RuleFor(x => x.Alias)
            .MaximumLength(250);

        RuleFor(x => x.Nationality)
            .MaximumLength(250);

        RuleFor(x => x.LastKnownLocation)
            .MaximumLength(250).WithMessage("Last Known Location must not exceed 250 characters");
    }
}
