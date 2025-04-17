using ShadowNetBackend.Features.Witnesses.GetByIdWitness;

namespace ShadowNetBackend.Features.Witnesses.GetWitnessById;

public class GetWitnessByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/witnesses/{id:guid}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(new GetWitnessByIdQuery(id), cancellationToken);
            return TypedResults.Ok(response);
        })
        .WithTags("Witnesses")
        .WithName("GetWitnessById")
        .WithDescription("Get witness by Id")
        .Produces<WitnessDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}