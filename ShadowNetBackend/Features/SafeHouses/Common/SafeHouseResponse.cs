namespace ShadowNetBackend.Features.SafeHouses.Common;

public record SafeHouseResponse(
    int Id,
    string Location,
    int Capacity,
    bool IsActive);
