using ShadowNetBackend.Features.SafeHouses.Common;
using ShadowNetBackend.Features.SafeHouses.GetAllSafeHouse;

namespace ShadowNetBackend.Features.SafeHouses.GetSafeHouses;

public class GetSafeHousesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/safehouses", async (
            [AsParameters] SafeHouseParameters parameters,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetSafeHousesQuery(parameters), cancellationToken);
            return TypedResults.Ok(result);
        })
        .WithTags("SafeHouses")
        .WithName("GetSafeHouses")
        .WithDescription("Get safe houses")
        .Produces<IEnumerable<SafeHouseDto>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}