using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Common;
using ShadowNetBackend.Features.SafeHouses.Common;
using ShadowNetBackend.Helpers;
using ShadowNetBackend.Infrastructure.Data;
using ShadowNetBackend.Infrastructure.Security;
using ShadowNetBackend.Mappings;

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


        return safeHouse.ToSafeHouseResponse();
    }
}
