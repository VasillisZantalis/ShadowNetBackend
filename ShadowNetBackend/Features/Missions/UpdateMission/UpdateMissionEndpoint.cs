namespace ShadowNetBackend.Features.Missions.UpdateMission;

public record UpdateMissionRequest(MissionForUpdateDto MissionForUpdate);

public class UpdateMissionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/missions", async (
            [FromBody] UpdateMissionRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            await sender.Send(new UpdateMissionCommand(request.MissionForUpdate), cancellationToken);
            return TypedResults.NoContent();
        })
        .WithName("UpdateMission")
        .WithDescription("Update an existing mission")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
