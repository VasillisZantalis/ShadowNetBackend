
using ShadowNetBackend.Dtos.Witnesses;

namespace ShadowNetBackend.Features.Witnesses.CreateWitness;

public class CreateWitnessEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/witnesses", async (
            [FromBody] WitnessForCreationDto witnessForCreation,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new CreateWitnessCommand(witnessForCreation), cancellationToken);
            return TypedResults.Created($"/api/witnesses/{result}", result);
        })
        .WithTags("Witnesses")
        .WithName("CreateWitness")
        .WithDescription("Create a new witness")
        .Produces<int>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}