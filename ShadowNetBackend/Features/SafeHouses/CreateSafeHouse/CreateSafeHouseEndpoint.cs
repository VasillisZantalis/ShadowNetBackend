
namespace ShadowNetBackend.Features.SafeHouses.CreateSafeHouse;

public class CreateSafeHouseEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/safehouses", async (
                [FromBody] SafeHouseForCreationDto safeHouseForCreation,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new CreateSafeHouseCommand(safeHouseForCreation), cancellationToken);
                return TypedResults.Created($"/api/safehouses/{result}", result);
            })
            .WithTags("SafeHouses")
            .WithName("CreateSafeHouse")
            .WithDescription("Create a new safe house")
            .Produces<int>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}