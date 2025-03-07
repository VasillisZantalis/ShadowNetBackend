using FluentValidation;

namespace ShadowNetBackend.Features.Agents.UpdateAgent;

public class UpdateAgentValidator : AbstractValidator<UpdateAgentCommand>
{
    public UpdateAgentValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(250).WithMessage("First name must not exceed 250 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(250).WithMessage("Last name must not exceed 250 characters");
    }
}
