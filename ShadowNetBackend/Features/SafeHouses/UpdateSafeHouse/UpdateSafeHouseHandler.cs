using ShadowNetBackend.Features.SafeHouses.Common;

namespace ShadowNetBackend.Features.SafeHouses.UpdateSafeHouse;

public record UpdateSafeHouseCommand(SafeHouseForUpdateDto SafeHouseForUpdate) : ICommand<bool>;

public class UpdateSafeHouseCommandValidator : AbstractValidator<UpdateSafeHouseCommand>
{
    public UpdateSafeHouseCommandValidator()
    {
        RuleFor(x => x.SafeHouseForUpdate.Location).NotEmpty().MaximumLength(200);
        RuleFor(x => x.SafeHouseForUpdate.Capacity).NotEmpty();
        RuleFor(x => x.SafeHouseForUpdate.IsActive).NotNull();
    }
}

internal class UpdateSafeHouseHandler(
    ApplicationDbContext dbContext,
    ICacheService cache) : ICommandHandler<UpdateSafeHouseCommand, bool>
{
    public async Task<bool> Handle(UpdateSafeHouseCommand request, CancellationToken cancellationToken)
    {
        var safeHouse = await dbContext.SafeHouses.FirstOrDefaultAsync(x => x.Id == request.SafeHouseForUpdate.Id, cancellationToken);
        if (safeHouse is null)
            throw new SafeHouseNotFoundException();

        safeHouse.Location = request.SafeHouseForUpdate.Location;
        safeHouse.Capacity = request.SafeHouseForUpdate.Capacity;
        safeHouse.IsActive = request.SafeHouseForUpdate.IsActive;

        dbContext.SafeHouses.Update(safeHouse);
        await dbContext.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(nameof(CacheKeys.SafeHouses));
        return true;
    }
}