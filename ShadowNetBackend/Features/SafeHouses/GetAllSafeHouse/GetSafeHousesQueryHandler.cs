using MediatR;
using Microsoft.EntityFrameworkCore;
using ShadowNetBackend.Extensions;
using ShadowNetBackend.Features.SafeHouses.Common;
using ShadowNetBackend.Helpers;
using ShadowNetBackend.Infrastructure.Data;
using ShadowNetBackend.Mappings;

namespace ShadowNetBackend.Features.SafeHouses.GetAllSafeHouse;

public class GetSafeHousesQueryHandler : IRequestHandler<GetSafeHousesQuery, IEnumerable<SafeHouseResponse>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetSafeHousesQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<SafeHouseResponse>> Handle(GetSafeHousesQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.SafeHouses.AsQueryable()
            .ApplySorting(request.Parameters.OrderBy)
            .ApplyPagination(request.Parameters.PageSize, request.Parameters.PageNumber);

        var SafeHouses = await query.ToListAsync(cancellationToken);

        return SafeHouses.Select(s => s.ToSafeHouseResponse()).ToList();
    }
}
