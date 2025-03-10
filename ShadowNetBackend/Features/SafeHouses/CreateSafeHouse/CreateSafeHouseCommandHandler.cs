using MediatR;
using ShadowNetBackend.Infrastructure.Data;

namespace ShadowNetBackend.Features.SafeHouses.CreateSafeHouse;

public class CreateSafeHouseCommandHandler : IRequestHandler<CreateSafeHouseCommand, int>
{
    private readonly ApplicationDbContext _dbContext;

    public CreateSafeHouseCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
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

        return safeHouse.Id;
    }
}
