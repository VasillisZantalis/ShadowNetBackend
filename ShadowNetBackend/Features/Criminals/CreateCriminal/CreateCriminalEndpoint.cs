namespace ShadowNetBackend.Features.Criminals.CreateCriminal;

public class CreateCriminalEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/criminals", async (
            [FromBody] CriminalForCreationDto criminalForCreation,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new CreateCriminalCommand(criminalForCreation), cancellationToken);
            return TypedResults.Created($"/api/criminals/{result}", result);
        })
        .WithTags("Criminals")
        .WithName("CreateCriminal")
        .WithDescription("Create a new criminal")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}