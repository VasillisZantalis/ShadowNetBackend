using FluentValidation;

namespace ShadowNetBackend.Features.Witnesses.CreateWitness;

public class CreateWitnessValidator : AbstractValidator<CreateWitnessCommand>
{
    public CreateWitnessValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(250).WithMessage("First name must not exceed 250 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(250).WithMessage("Last name must not exceed 250 characters");

        RuleFor(x => x.Alias)
           .NotEmpty();

        RuleFor(x => x.RiskLevel)
           .NotEmpty().WithMessage("Risk Level is required");
    }
}
