namespace ShadowNetBackend.Features.SafeHouses.CreateSafeHouse;

public record CreateSafeHouseCommand(SafeHouseForCreationDto SafeHouseForCreation) : ICommand<int>;

public class CreateSafeHouseCommandValidator : AbstractValidator<CreateSafeHouseCommand>
{
    public CreateSafeHouseCommandValidator()
    {
        RuleFor(x => x.SafeHouseForCreation.Location).NotEmpty().MaximumLength(200);
        RuleFor(x => x.SafeHouseForCreation.Capacity).NotEmpty();
        RuleFor(x => x.SafeHouseForCreation.IsActive).NotNull();
    }
}

internal class CreateSafeHouseHandler(
    ApplicationDbContext dbContext,
    ICacheService cache) : ICommandHandler<CreateSafeHouseCommand, int>
{
    public async Task<int> Handle(CreateSafeHouseCommand request, CancellationToken cancellationToken)
    {
        var safeHouse = request.SafeHouseForCreation.ToSafeHouse();

        dbContext.SafeHouses.Add(safeHouse);
        await dbContext.SaveChangesAsync();

        await cache.RemoveAsync(nameof(CacheKeys.SafeHouses));
        return safeHouse.Id;
    }
}