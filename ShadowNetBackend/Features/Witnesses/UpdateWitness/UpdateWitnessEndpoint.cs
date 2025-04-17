namespace ShadowNetBackend.Features.Witnesses.UpdateWitness;

public class UpdateWitnessEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/witnesses", async (
            [FromBody] WitnessForUpdateDto witnessForUpdate,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            await sender.Send(new UpdateWitnessCommand(witnessForUpdate), cancellationToken);
            return TypedResults.NoContent();
        })
        .WithTags("Witnesses")
        .WithName("UpdateWitness")
        .WithDescription("Update an existing witness")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}