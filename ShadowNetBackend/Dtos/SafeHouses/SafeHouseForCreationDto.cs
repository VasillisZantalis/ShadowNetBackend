namespace ShadowNetBackend.Dtos.SafeHouses;

public record SafeHouseForCreationDto(
    string Location,
    int Capacity,
    bool IsActive);