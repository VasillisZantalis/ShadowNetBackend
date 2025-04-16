using ShadowNetBackend.Features.Criminals.Common;
using ShadowNetBackend.Features.Criminals.GetAllCriminals;

namespace ShadowNetBackend.Features.Criminals.GetCriminals;

public class GetCriminalsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/criminals", async (
            [AsParameters] CriminalParameters parameters,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetCriminalsQuery(parameters), cancellationToken);
            return TypedResults.Ok(result);
        })
        .WithName("GetCriminals")
        .WithDescription("Get criminals")
        .Produces<IEnumerable<CriminalDto>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
