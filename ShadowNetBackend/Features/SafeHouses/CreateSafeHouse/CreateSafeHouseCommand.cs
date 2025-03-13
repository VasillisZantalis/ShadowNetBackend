namespace ShadowNetBackend.Features.SafeHouses.CreateSafeHouse;

public record CreateSafeHouseCommand(
    string Location,
    int Capacity,
    bool IsActive) : IRequest<int>;
