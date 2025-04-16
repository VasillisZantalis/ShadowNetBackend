using ShadowNetBackend.Features.Criminals.Common;

namespace ShadowNetBackend.Features.Criminals.UpdateCriminal;

public record UpdateCriminalCommand(CriminalForUpdateDto CriminalForUpdate) : ICommand<bool>;

public class UpdateCriminalCommandValidator : AbstractValidator<UpdateCriminalCommand>
{
    public UpdateCriminalCommandValidator()
    {
        RuleFor(x => x.CriminalForUpdate.FirstName)
           .NotEmpty().WithMessage("First name is required")
           .MaximumLength(250).WithMessage("First name must not exceed 250 characters");

        RuleFor(x => x.CriminalForUpdate.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(250).WithMessage("Last name must not exceed 250 characters");

        RuleFor(x => x.CriminalForUpdate.ThreatLevel)
            .IsInEnum().WithMessage("Threat level is invalid");

        RuleFor(x => x.CriminalForUpdate.Alias)
            .MaximumLength(250);

        RuleFor(x => x.CriminalForUpdate.Nationality)
            .MaximumLength(250);

        RuleFor(x => x.CriminalForUpdate.LastKnownLocation)
            .MaximumLength(250).WithMessage("Last Known Location must not exceed 250 characters");
    }
}


internal class UpdateCriminalHandler(
    ApplicationDbContext dbContext,
    ICacheService cache) : ICommandHandler<UpdateCriminalCommand, bool>
{
    public async Task<bool> Handle(UpdateCriminalCommand request, CancellationToken cancellationToken)
    {
        var criminal = await dbContext.Criminals.FirstOrDefaultAsync(c => c.Id == request.CriminalForUpdate.Id, cancellationToken);
        if (criminal is null)
            throw new CriminalNotFoundException();

        criminal.FirstName = request.CriminalForUpdate.FirstName;
        criminal.LastName = request.CriminalForUpdate.LastName;
        criminal.Alias = request.CriminalForUpdate.Alias;
        criminal.DateOfBirth = request.CriminalForUpdate.DateOfBirth;
        criminal.Nationality = request.CriminalForUpdate.Nationality;
        criminal.KnownAffiliations = request.CriminalForUpdate.KnownAffiliations;
        criminal.ThreatLevel = request.CriminalForUpdate.ThreatLevel;
        criminal.IsArmedAndDangerous = request.CriminalForUpdate.IsArmedAndDangerous;
        criminal.LastKnownLocation = request.CriminalForUpdate.LastKnownLocation;
        criminal.LastSpottedDate = request.CriminalForUpdate.LastSpottedDate;
        criminal.UnderSurveillance = request.CriminalForUpdate.UnderSurveillance;
        criminal.SurveillanceNotes = request.CriminalForUpdate.SurveillanceNotes;
        criminal.Image = request.CriminalForUpdate.Image != null ? FileHelper.ConvertFromBase64(request.CriminalForUpdate.Image) : null;

        await dbContext.SaveChangesAsync(cancellationToken);
        await cache.RemoveAsync(nameof(CacheKeys.Criminals));

        return true;
    }
}
