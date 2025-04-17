using ShadowNetBackend.Features.Agents.GetByIdAgent;

namespace ShadowNetBackend.Features.Agents.GetAgentById;

public class GetAgentByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/agents/{id:guid}", async (
            Guid id, 
            ISender sender, 
            CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(new GetAgentByIdQuery(id), cancellationToken);
            return TypedResults.Ok(response);
        })
        .WithTags("Agents")
        .WithName("GetAgentById")
        .WithDescription("Get agent by Id")
        .Produces<AgentDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}
