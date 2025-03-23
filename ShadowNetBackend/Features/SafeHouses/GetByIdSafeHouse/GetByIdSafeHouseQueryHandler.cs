using ShadowNetBackend.Features.SafeHouses.Common;

namespace ShadowNetBackend.Features.SafeHouses.GetByIdSafeHouse;

public class GetByIdSafeHouseQueryHandler : IRequestHandler<GetByIdSafeHouseQuery, SafeHouseResponse>
{
    private readonly ApplicationDbContext _dbContext;

    public GetByIdSafeHouseQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SafeHouseResponse> Handle(GetByIdSafeHouseQuery request, CancellationToken cancellationToken)
    {
        var safeHouse = await _dbContext.SafeHouses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (safeHouse == null)
            throw new SafeHouseNotFoundException();

        var safeHouseResponse = safeHouse.ToSafeHouseResponse();

        return safeHouseResponse;
    }
}
