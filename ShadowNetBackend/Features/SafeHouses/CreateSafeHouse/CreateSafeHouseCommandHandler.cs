using MediatR;
using ShadowNetBackend.Common;
using ShadowNetBackend.Infrastructure.Data;
using ShadowNetBackend.Infrastructure.Interfaces;

namespace ShadowNetBackend.Features.SafeHouses.CreateSafeHouse;

public class CreateSafeHouseCommandHandler : IRequestHandler<CreateSafeHouseCommand, int>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;

    public CreateSafeHouseCommandHandler(ApplicationDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<int> Handle(CreateSafeHouseCommand request, CancellationToken cancellationToken)
    {
        var safeHouse = new SafeHouse
        {
            Location = request.Location,
            Capacity = request.Capacity,
            IsActive = request.IsActive
        };

        _dbContext.SafeHouses.Add(safeHouse);
        await _dbContext.SaveChangesAsync();

        await _cache.RemoveAsync(nameof(CacheKeys.SafeHouses));

        return safeHouse.Id;
    }
}
