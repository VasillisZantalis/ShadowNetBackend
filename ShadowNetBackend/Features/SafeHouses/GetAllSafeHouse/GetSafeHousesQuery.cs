using ShadowNetBackend.Features.SafeHouses.Common;

namespace ShadowNetBackend.Features.SafeHouses.GetAllSafeHouse;

public record GetSafeHousesQuery(SafeHouseParameters Parameters) : IRequest<IEnumerable<SafeHouseResponse>>;
