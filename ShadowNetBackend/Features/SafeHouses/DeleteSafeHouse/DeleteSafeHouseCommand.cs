using MediatR;

namespace ShadowNetBackend.Features.SafeHouses.DeleteSafeHouse;

public record DeleteSafeHouseCommand(int Id) : IRequest<bool>;
