using FluentValidation;

namespace ShadowNetBackend.Features.Witnesses.UpdateWitness;

public class UpdateWitnessValidator : AbstractValidator<UpdateWitnessCommand>
{
    public UpdateWitnessValidator()
    {
        RuleFor(x => x.RiskLevel)
           .NotEmpty().WithMessage("Risk Level is required");
    }
}
