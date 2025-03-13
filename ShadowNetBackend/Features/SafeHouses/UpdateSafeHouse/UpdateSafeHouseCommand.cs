namespace ShadowNetBackend.Features.SafeHouses.UpdateSafeHouse;

public record UpdateSafeHouseCommand(
    int Id,
    string Location,
    int Capacity,
    bool IsActive) : IRequest<bool>;
