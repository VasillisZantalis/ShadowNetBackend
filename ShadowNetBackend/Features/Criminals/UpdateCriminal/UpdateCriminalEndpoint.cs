namespace ShadowNetBackend.Features.Criminals.UpdateCriminal;

public record UpdateCriminalRequest(CriminalForUpdateDto CriminalForUpdate);

public class UpdateCriminalEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/criminals", async (
            [FromBody] UpdateCriminalRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            await sender.Send(new UpdateCriminalCommand(request.CriminalForUpdate), cancellationToken);
            return TypedResults.NoContent();
        })
        .WithName("UpdateCriminal")
        .WithDescription("Update an existing criminal")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
