namespace ShadowNetBackend.Dtos.SafeHouses;

public record SafeHouseForUpdateDto(
    int Id,
    string Location,
    int Capacity,
    bool IsActive);