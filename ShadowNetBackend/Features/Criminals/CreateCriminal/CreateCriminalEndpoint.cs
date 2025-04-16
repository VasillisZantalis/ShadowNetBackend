namespace ShadowNetBackend.Features.Criminals.CreateCriminal;

public record class CreateCriminalRequest(CriminalForCreationDto CriminalForCreation);

public class CreateCriminalEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/criminals", async (
            [FromBody] CreateCriminalRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new CreateCriminalCommand(request.CriminalForCreation), cancellationToken);
            return TypedResults.Created($"/api/criminals/{result.Id}", result);
        })
        .WithName("CreateCriminal")
        .WithDescription("Create a new criminal")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
