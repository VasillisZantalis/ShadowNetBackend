namespace ShadowNetBackend.Features.SafeHouses.DeleteSafeHouse;

public class DeleteSafeHouseEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/safehouses/{id:int}", async (
        int id,
        ISender sender,
        CancellationToken cancellationToken) =>
        {
            await sender.Send(new DeleteSafeHouseCommand(id), cancellationToken);
            return TypedResults.NoContent();
        })
        .WithTags("SafeHouses")
        .WithName("DeleteSafeHouse")
        .WithDescription("Delete safe house")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}