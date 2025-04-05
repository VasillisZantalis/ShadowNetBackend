using ShadowNetBackend.Features.Witnesses.Common;
using ShadowNetBackend.Features.Witnesses.CreateWitness;
using ShadowNetBackend.Features.Witnesses.DeleteWitness;
using ShadowNetBackend.Features.Witnesses.GetAllWitnesses;
using ShadowNetBackend.Features.Witnesses.GetByIdWitness;
using ShadowNetBackend.Features.Witnesses.UpdateWitness;

namespace ShadowNetBackend.Features.Witnesses;

public static class WitnessEndpoints
{
    public static void MapWitnessEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/Witnesses").WithTags("Witnesses");

        group.MapGet("", GetAllWitnesses)
            .WithName("GetAllWitnesses")
            .Produces<IEnumerable<WitnessResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapGet("/{id:guid}", GetWitnessById)
            .WithName("GetWitnessById")
            .Produces<WitnessResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("", CreateWitness)
            .WithName("CreateWitness")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id:guid}", UpdateWitness)
            .WithName("UpdateWitness")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:guid}", DeleteWitness)
            .WithName("DeleteWitness")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetAllWitnesses([AsParameters] WitnessParameters parameters, ISender sender, CancellationToken cancellationToken)
    {
        if (parameters is null)
        {
            return TypedResults.BadRequest("Invalid parameters");
        }

        var Witnesss = await sender.Send(new GetWitnessesQuery(parameters), cancellationToken);

        return TypedResults.Ok(Witnesss);
    }

    private static async Task<IResult> GetWitnessById(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        var Witness = await sender.Send(new GetByIdWitnessQuery(id), cancellationToken);

        return TypedResults.Ok(Witness);
    }

    private static async Task<IResult> CreateWitness(CreateWitnessCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var id = await sender.Send(command, cancellationToken);

        return id is null
            ? TypedResults.BadRequest("Failed to create Witness")
            : TypedResults.Created($"/api/Witnesss/{id}", id);
    }

    private static async Task<IResult> UpdateWitness(Guid id, UpdateWitnessCommand command, ISender sender, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return TypedResults.BadRequest("Id mismatch");

        var success = await sender.Send(command, cancellationToken);

        return !success
            ? TypedResults.BadRequest("Failed to update Witness")
            : TypedResults.NoContent();
    }

    private static async Task<IResult> DeleteWitness(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteWitnessCommand(id), cancellationToken);

        return TypedResults.NoContent();
    }
}
