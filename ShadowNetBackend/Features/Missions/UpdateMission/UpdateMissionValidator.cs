namespace ShadowNetBackend.Features.Missions.UpdateMission;

public class UpdateMissionValidator : AbstractValidator<UpdateMissionCommand>
{
    public UpdateMissionValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty();

        RuleFor(x => x.Risk)
            .NotEmpty();
    }
}
