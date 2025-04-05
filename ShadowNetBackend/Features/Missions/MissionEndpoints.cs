using Microsoft.AspNetCore.Mvc;
using ShadowNetBackend.Features.Missions.Common;
using ShadowNetBackend.Features.Missions.CreateMission;
using ShadowNetBackend.Features.Missions.DeleteMission;
using ShadowNetBackend.Features.Missions.GetAllMissions;
using ShadowNetBackend.Features.Missions.GetByIdMission;
using ShadowNetBackend.Features.Missions.UpdateMission;

namespace ShadowNetBackend.Features.Missions;

public static class MissionEndpoints
{
    public static void MapMissionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/missions").WithTags("Missions");

        group.MapGet("", GetAllMissions)
            .WithName("GetAllMissions")
            .Produces<IEnumerable<MissionResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapGet("/{id:guid}", GetMissionById)
            .WithName("GetMissionById")
            .Produces<MissionResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("", CreateMission)
            .WithName("CreateMission")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id:guid}", UpdateMission)
            .WithName("UpdateMission")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:guid}", DeleteMission)
            .WithName("DeleteMission")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetAllMissions([AsParameters] MissionParameters parameters, ISender sender, CancellationToken cancellationToken)
    {
        if (parameters is null)
            return TypedResults.BadRequest("Invalid Parameters");

        var missions = await sender.Send(new GetMissionsQuery(parameters), cancellationToken);
        return TypedResults.Ok(missions);
    }

    private static async Task<IResult> GetMissionById(
        Guid id,
        [FromQuery] EncryptionType? encryptionType,
        [FromQuery] string? encryptionKey,
        ISender sender,
        CancellationToken cancellation)
    {
        var mission = await sender.Send(new GetByIdMissionQuery(id, encryptionType, encryptionKey), cancellation);
        return TypedResults.Ok(mission);
    }

    private static async Task<IResult> CreateMission(CreateMissionCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var id = await sender.Send(command, cancellationToken);

        return TypedResults.Created($"/api/missions/{id}", id);
    }

    private static async Task<IResult> UpdateMission(Guid id, UpdateMissionCommand command, ISender sender, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return TypedResults.BadRequest("Id mismatch");

        await sender.Send(command, cancellationToken);

        return TypedResults.NoContent();
    }

    private static async Task<IResult> DeleteMission(Guid id, ISender sender, CancellationToken cancellation)
    {
        await sender.Send(new DeleteMissionCommand(id), cancellation);
        return TypedResults.NoContent();
    }
}
