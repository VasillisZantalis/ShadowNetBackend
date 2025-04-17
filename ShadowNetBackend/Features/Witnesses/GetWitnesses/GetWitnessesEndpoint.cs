using ShadowNetBackend.Features.Witnesses.Common;
using ShadowNetBackend.Features.Witnesses.GetAllWitnesses;

namespace ShadowNetBackend.Features.Witnesses.GetWitnesses;

public class GetWitnessesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/witnesses", async (
            [AsParameters] WitnessParameters parameters,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetWitnessesQuery(parameters), cancellationToken);
            return TypedResults.Ok(result);
        })
        .WithTags("Witnesses")
        .WithName("GetWitnesses")
        .WithDescription("Get witnesses")
        .Produces<IEnumerable<WitnessDto>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}