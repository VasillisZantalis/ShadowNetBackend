using ShadowNetBackend.Features.SafeHouses.Common;
using ShadowNetBackend.Features.SafeHouses.CreateSafeHouse;
using ShadowNetBackend.Features.SafeHouses.DeleteSafeHouse;
using ShadowNetBackend.Features.SafeHouses.GetAllSafeHouse;
using ShadowNetBackend.Features.SafeHouses.GetByIdSafeHouse;
using ShadowNetBackend.Features.SafeHouses.UpdateSafeHouse;

namespace ShadowNetBackend.Features.SafeHouses;

public static class SafeHouseEndpoints
{
    public static void MapSafeHouseEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/SafeHouses").WithTags("SafeHouses");

        group.MapGet("", GetAllSafeHouses)
            .WithName("GetAllSafeHouses")
            .Produces<IEnumerable<SafeHouse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapGet("/{id:int}", GetSafeHouseById)
            .WithName("GetSafeHouseById")
            .Produces<SafeHouse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("", CreateSafeHouse)
            .WithName("CreateSafeHouse")
            .Produces<int>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id:int}", UpdateSafeHouse)
            .WithName("UpdateSafeHouse")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:int}", DeleteSafeHouse)
            .WithName("DeleteSafeHouse")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetAllSafeHouses([AsParameters] SafeHouseParameters parameters, ISender sender, CancellationToken cancellationToken)
    {
        if (parameters is null)
            return TypedResults.BadRequest("Invalid Parameters");

        var SafeHouses = await sender.Send(new GetSafeHousesQuery(parameters), cancellationToken);
        return TypedResults.Ok(SafeHouses);
    }

    private static async Task<IResult> GetSafeHouseById(int id, ISender sender, CancellationToken cancellation)
    {
        var SafeHouse = await sender.Send(new GetByIdSafeHouseQuery(id), cancellation);
        return TypedResults.Ok(SafeHouse);
    }

    private static async Task<IResult> CreateSafeHouse(CreateSafeHouseCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var id = await sender.Send(command, cancellationToken);

        return TypedResults.Created($"/api/SafeHouses/{id}", id);
    }

    private static async Task<IResult> UpdateSafeHouse(int id, UpdateSafeHouseCommand command, ISender sender, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return TypedResults.BadRequest("Id mismatch");

        await sender.Send(command, cancellationToken);

        return TypedResults.NoContent();
    }

    private static async Task<IResult> DeleteSafeHouse(int id, ISender sender, CancellationToken cancellation)
    {
        await sender.Send(new DeleteSafeHouseCommand(id), cancellation);
        return TypedResults.NoContent();
    }
}
