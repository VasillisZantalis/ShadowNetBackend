namespace ShadowNetBackend.Features.Missions.UpdateMission;

public class UpdateMissionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/missions", async (
            [FromBody] MissionForUpdateDto missionForUpdate,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            await sender.Send(new UpdateMissionCommand(missionForUpdate), cancellationToken);
            return TypedResults.NoContent();
        })
        .WithName("UpdateMission")
        .WithDescription("Update an existing mission")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
