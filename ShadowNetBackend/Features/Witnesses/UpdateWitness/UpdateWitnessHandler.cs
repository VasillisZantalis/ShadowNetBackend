using ShadowNetBackend.Features.Witnesses.Common;

namespace ShadowNetBackend.Features.Witnesses.UpdateWitness;

public record UpdateWitnessCommand(WitnessForUpdateDto WitnessForUpdate) : ICommand<bool>;

public class UpdateWitnessCommandValidator : AbstractValidator<UpdateWitnessCommand>
{
    public UpdateWitnessCommandValidator()
    {
        RuleFor(x => x.WitnessForUpdate.RiskLevel).NotEmpty().WithMessage("Risk Level is required");
    }
}

internal class UpdateWitnessHandler(
    ApplicationDbContext dbContext,
    ICacheService cache) : ICommandHandler<UpdateWitnessCommand, bool>
{
    public async Task<bool> Handle(UpdateWitnessCommand request, CancellationToken cancellationToken)
    {
        var witness = await dbContext.Witnesses.FirstOrDefaultAsync(w => w.Id == request.WitnessForUpdate.Id, cancellationToken);
        if (witness is null)
            throw new WitnessNotFoundException();

        witness.Alias = request.WitnessForUpdate.Alias ?? string.Empty;
        witness.Image = request.WitnessForUpdate.Image is null
                ? null
                : FileHelper.ConvertFromBase64(request.WitnessForUpdate.Image);
        witness.RiskLevel = request.WitnessForUpdate.RiskLevel;
        witness.LocationHistory = request.WitnessForUpdate.LocationHistory;
        witness.RelocationStatus = request.WitnessForUpdate.RelocationStatus;

        await dbContext.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(nameof(CacheKeys.Witnesses));
        return true;
    }
}