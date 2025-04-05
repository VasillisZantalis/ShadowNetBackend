using ShadowNetBackend.Features.Criminals.Common;
using ShadowNetBackend.Features.Criminals.CreateCriminal;
using ShadowNetBackend.Features.Criminals.DeleteCriminal;
using ShadowNetBackend.Features.Criminals.GetAllCriminals;
using ShadowNetBackend.Features.Criminals.GetByIdCriminal;
using ShadowNetBackend.Features.Criminals.UpdateCriminal;

namespace ShadowNetBackend.Features.Criminals;

public static class CriminalEndpoints
{
    public static void MapCriminalEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/criminals").WithTags("Criminals");

        group.MapGet("", GetAllCriminals)
            .WithName("GetAllCriminals")
            .Produces<IEnumerable<CriminalResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapGet("/{id:guid}", GetCriminalById)
            .WithName("GetCriminalById")
            .Produces<CriminalResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("", CreateCriminal)
            .WithName("CreateCriminal")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id:guid}", UpdateCriminal)
            .WithName("UpdateCriminal")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:guid}", DeleteCriminal)
            .WithName("DeleteCriminal")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetAllCriminals([AsParameters] CriminalParameters parameters, ISender sender, CancellationToken cancellationToken)
    {
        if (parameters is null)
        {
            return TypedResults.BadRequest("Invalid parameters");
        }

        var criminals = await sender.Send(new GetCriminalsQuery(parameters), cancellationToken);

        return TypedResults.Ok(criminals);
    }

    private static async Task<IResult> GetCriminalById(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        var criminal = await sender.Send(new GetByIdCriminalQuery(id), cancellationToken);

        return TypedResults.Ok(criminal);
    }

    private static async Task<IResult> CreateCriminal(CreateCriminalCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var id = await sender.Send(command, cancellationToken);

        return id is null
            ? TypedResults.BadRequest("Failed to create criminal")
            : TypedResults.Created($"/api/criminals/{id}", id);
    }

    private static async Task<IResult> UpdateCriminal(Guid id, UpdateCriminalCommand command, ISender sender, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return TypedResults.BadRequest("Id mismatch");

        var success = await sender.Send(command, cancellationToken);

        return !success
            ? TypedResults.BadRequest("Failed to update criminal")
            : TypedResults.NoContent();
    }

    private static async Task<IResult> DeleteCriminal(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteCriminalCommand(id), cancellationToken);

        return TypedResults.NoContent();
    }
}
