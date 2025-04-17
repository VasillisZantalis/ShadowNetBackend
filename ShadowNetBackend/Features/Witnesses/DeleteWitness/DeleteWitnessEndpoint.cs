namespace ShadowNetBackend.Features.Witnesses.DeleteWitness;

public class DeleteWitnessEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/witnesses/{id:guid}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            await sender.Send(new DeleteWitnessCommand(id), cancellationToken);
            return TypedResults.NoContent();
        })
        .WithTags("Witnesses")
        .WithName("DeleteWitness")
        .WithDescription("Delete witness")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}