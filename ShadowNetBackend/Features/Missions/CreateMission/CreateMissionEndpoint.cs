namespace ShadowNetBackend.Features.Missions.CreateMission;

public record CreateMissionRequest(MissionForCreationDto MissionForCreation);

public class CreateMissionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/missions", async (
            [FromBody] MissionForCreationDto missionForCreation,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new CreateMissionCommand(missionForCreation), cancellationToken);
            return TypedResults.Created($"/api/missions/{result}", result);
        })
        .WithName("CreateMission")
        .WithDescription("Create a new mission")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
