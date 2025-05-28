namespace ShadowNetBackend.Features.Missions.CreateMission;

public record CreateMissionCommand(MissionForCreationDto MissionForCreation) : ICommand<Guid>;

public class CreateMissionCommandValidator : AbstractValidator<CreateMissionCommand>
{
    public CreateMissionCommandValidator()
    {
        RuleFor(x => x.MissionForCreation.Title).NotEmpty().MaximumLength(100);

        RuleFor(x => x.MissionForCreation.Objective).NotEmpty();

        RuleFor(x => x.MissionForCreation.Location).NotEmpty().MaximumLength(200);

        RuleFor(x => x.MissionForCreation.Status).NotEmpty();

        RuleFor(x => x.MissionForCreation.Risk).NotEmpty();

        RuleFor(x => x.MissionForCreation.Date).NotEmpty();

        RuleFor(x => x.MissionForCreation.EncryptionType)
            .Must(value => Enum.IsDefined(typeof(EncryptionType), value))
            .WithMessage("Incorrect Encryption type");
    }
}


internal class CreateMissionHandler(
    ApplicationDbContext dbContext,
    ICryptographyService cryptographyService,
    ICacheService cache) : ICommandHandler<CreateMissionCommand, Guid>
{
    public async Task<Guid> Handle(CreateMissionCommand command, CancellationToken cancellationToken)
    {
        var mission = new Mission
        {
            Title = command.MissionForCreation.Title,
            Image = command.MissionForCreation.Image != null
                ? FileHelper.ConvertFromBase64(command.MissionForCreation.Image)
                : null,
            Objective = command.MissionForCreation.EncryptionType != EncryptionType.None
                ? cryptographyService.Encrypt(command.MissionForCreation.Objective, command.MissionForCreation.EncryptionType, command.MissionForCreation.EncryptionKey)
                : command.MissionForCreation.Objective,
            Location = command.MissionForCreation.EncryptionType != EncryptionType.None
                ? cryptographyService.Encrypt(command.MissionForCreation.Location, command.MissionForCreation.EncryptionType, command.MissionForCreation.EncryptionKey)
                : command.MissionForCreation.Location,
            Status = command.MissionForCreation.Status,
            Risk = command.MissionForCreation.Risk,
            Date = command.MissionForCreation.Date
        };

        dbContext.Missions.Add(mission);
        await dbContext.SaveChangesAsync();

        await cache.RemoveAsync(nameof(CacheKeys.Missions));

        return mission.Id;
    }
}
