namespace ShadowNetBackend.Features.Missions.CreateMission;

public class CreateMissionValidator : AbstractValidator<CreateMissionCommand>
{
    public CreateMissionValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Objective)
            .NotEmpty();

        RuleFor(x => x.Location)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Status)
            .NotEmpty();

        RuleFor(x => x.Risk)
            .NotEmpty();

        RuleFor(x => x.Date)
            .NotEmpty();

        RuleFor(x => x.EncryptionType)
            .NotEmpty().WithMessage("Encryption type is required");
    }
}
