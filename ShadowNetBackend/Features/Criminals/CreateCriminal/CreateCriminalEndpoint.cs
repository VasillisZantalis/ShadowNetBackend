namespace ShadowNetBackend.Features.Criminals.CreateCriminal;

public record class CreateCriminalRequest(CriminalForCreationDto CriminalForCreation);
public record class CreateCriminalResponse(Guid Id);

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
            var response = new CreateCriminalResponse(result.Id);
            return TypedResults.Created($"/api/criminals/{response.Id}", response);
        })
        .WithName("CreateCriminal")
        .WithDescription("Create a new criminal")
        .Produces<CreateCriminalResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
