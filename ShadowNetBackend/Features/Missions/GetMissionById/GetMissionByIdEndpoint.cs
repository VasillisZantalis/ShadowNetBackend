using ShadowNetBackend.Features.Missions.GetByIdMission;

namespace ShadowNetBackend.Features.Missions.GetMissionById;

public class GetMissionByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/missions/{id:guid}", async (
            Guid id,
            [FromQuery] EncryptionType? encryptionType,
            [FromQuery] string? encryptionKey,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(new GetByIdMissionQuery(id, encryptionType, encryptionKey), cancellationToken);
            return TypedResults.Ok(response);
        })
        .WithName("GetMissionById")
        .WithDescription("Get mission by Id")
        .Produces<MissionDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}
