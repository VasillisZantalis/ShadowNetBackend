using ShadowNetBackend.Features.SafeHouses.Common;

namespace ShadowNetBackend.Features.SafeHouses.GetByIdSafeHouse;
public record GetSafeHouseByIdQuery(int Id) : IQuery<SafeHouseDto>;

internal class GetSafeHouseByIdHandler(
    ApplicationDbContext dbContext) : IQueryHandler<GetSafeHouseByIdQuery, SafeHouseDto>
{
    public async Task<SafeHouseDto> Handle(GetSafeHouseByIdQuery request, CancellationToken cancellationToken)
    {
        var safeHouse = await dbContext.SafeHouses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (safeHouse == null)
            throw new SafeHouseNotFoundException();

        return safeHouse.ToSafeHouseDto();
    }
}