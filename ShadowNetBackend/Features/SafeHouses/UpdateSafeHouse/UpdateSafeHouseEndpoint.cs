
namespace ShadowNetBackend.Features.SafeHouses.UpdateSafeHouse;

public class UpdateSafeHouseEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/safehouses", async (
            [FromBody] SafeHouseForUpdateDto safeHouseForUpdate,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            await sender.Send(new UpdateSafeHouseCommand(safeHouseForUpdate), cancellationToken);
            return TypedResults.NoContent();
        })
        .WithTags("SafeHouses")
        .WithName("UpdateSafeHouse")
        .WithDescription("Update an existing safe house")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}