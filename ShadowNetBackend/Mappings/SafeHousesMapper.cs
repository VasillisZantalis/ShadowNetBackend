namespace ShadowNetBackend.Mappings;

public static class SafeHousesMapper
{
    public static SafeHouseDto ToSafeHouseDto(this SafeHouse safeHouse) => new SafeHouseDto(
            safeHouse.Id,
            safeHouse.Location,
            safeHouse.Capacity,
            safeHouse.IsActive
        );

    public static IEnumerable<SafeHouseDto> ToSafeHouseDto(this IEnumerable<SafeHouse> safeHouses) => safeHouses.Select(s => s.ToSafeHouseDto());

    public static SafeHouse ToSafeHouse(this SafeHouseForCreationDto safeHouseForCreation) => new SafeHouse
        {
            Location = safeHouseForCreation.Location,
            Capacity = safeHouseForCreation.Capacity,
            IsActive = safeHouseForCreation.IsActive
        };
}
