using ShadowNetBackend.Exceptions;

namespace ShadowNetBackend.Features.SafeHouses.Common;

public class SafeHouseNotFoundException : NotFoundException
{
    public SafeHouseNotFoundException(string message = "SafeHouse was not found")
        : base(message) { }
}
