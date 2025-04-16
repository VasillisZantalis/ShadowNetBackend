namespace ShadowNetBackend.Features.Criminals.DeleteCriminal;

public class DeleteCriminalEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/criminals/{id:guid}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            await sender.Send(new DeleteCriminalCommand(id), cancellationToken);
            return TypedResults.NoContent();
        })
        .WithName("DeleteCriminal")
        .WithDescription("Delete criminal")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
