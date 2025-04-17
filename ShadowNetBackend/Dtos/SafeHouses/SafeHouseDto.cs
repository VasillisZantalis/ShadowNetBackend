namespace ShadowNetBackend.Dtos.SafeHouses;

public record SafeHouseDto(
    int Id,
    string Location,
    int Capacity,
    bool IsActive);