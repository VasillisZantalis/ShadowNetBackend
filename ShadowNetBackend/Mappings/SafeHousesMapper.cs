using ShadowNetBackend.Features.SafeHouses.Common;

namespace ShadowNetBackend.Mappings;

public static class SafeHousesMapper
{
    public static SafeHouseResponse ToSafeHouseResponse(this SafeHouse safeHouse)
    {
        return new SafeHouseResponse
        (
            safeHouse.Id,
            safeHouse.Location,
            safeHouse.Capacity,
            safeHouse.IsActive
        );
    }
}
