using ShadowNetBackend.Features.Criminals.GetByIdCriminal;

namespace ShadowNetBackend.Features.Criminals.GetCriminalById;
public record GetCriminalByIdResponse(CriminalDto Criminal);

public class GetCriminalByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/criminals/{id:guid}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(new GetCriminalByIdQuery(id), cancellationToken);
            return TypedResults.Ok(response);
        })
        .WithName("GetCriminalById")
        .WithDescription("Get criminal by Id")
        .Produces<GetCriminalByIdResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}