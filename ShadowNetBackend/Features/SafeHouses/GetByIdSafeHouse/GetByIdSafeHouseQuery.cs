using MediatR;
using ShadowNetBackend.Features.SafeHouses.Common;

namespace ShadowNetBackend.Features.SafeHouses.GetByIdSafeHouse;

public record GetByIdSafeHouseQuery(int Id) : IRequest<SafeHouseResponse>;
