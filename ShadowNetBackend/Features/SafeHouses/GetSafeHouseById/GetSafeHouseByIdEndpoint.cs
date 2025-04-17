using ShadowNetBackend.Features.SafeHouses.GetByIdSafeHouse;

namespace ShadowNetBackend.Features.SafeHouses.GetSafeHouseById;

public class GetSafeHouseByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/safehouses/{id:int}", async (
            int id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(new GetSafeHouseByIdQuery(id), cancellationToken);
            return TypedResults.Ok(response);
        })
        .WithTags("SafeHouses")
        .WithName("GetSafeHouseById")
        .WithDescription("Get safe house by Id")
        .Produces<SafeHouseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}