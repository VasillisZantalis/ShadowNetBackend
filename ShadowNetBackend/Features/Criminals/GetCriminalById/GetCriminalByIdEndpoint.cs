using ShadowNetBackend.Features.Criminals.GetByIdCriminal;

namespace ShadowNetBackend.Features.Criminals.GetCriminalById;

public class GetCriminalByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/criminals/{id:guid}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetCriminalByIdQuery(id), cancellationToken);
            return TypedResults.Ok(result);
        })
        .WithTags("Criminals")
        .WithName("GetCriminalById")
        .WithDescription("Get criminal by Id")
        .Produces<CriminalDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}