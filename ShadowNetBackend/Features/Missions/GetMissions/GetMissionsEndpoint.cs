using ShadowNetBackend.Features.Missions.Common;
using ShadowNetBackend.Features.Missions.GetAllMissions;

namespace ShadowNetBackend.Features.Missions.GetMissions;

public class GetMissionsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/missions", async (
            [AsParameters] MissionParameters parameters,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetMissionsQuery(parameters), cancellationToken);
            return TypedResults.Ok(result);
        })
        .WithName("GetMissions")
        .WithDescription("Get missions")
        .Produces<IEnumerable<MissionDto>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
