namespace ShadowNetBackend.Features.Missions.DeleteMission;

public class DeleteMissionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/missions/{id:guid}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            await sender.Send(new DeleteMissionCommand(id), cancellationToken);
            return TypedResults.NoContent();
        })
        .WithTags("Messages")
        .WithName("DeleteMission")
        .WithDescription("Delete mission")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}