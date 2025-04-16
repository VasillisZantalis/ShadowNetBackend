using ShadowNetBackend.Features.Agents.Common;
using ShadowNetBackend.Features.Agents.GetAllAgents;

namespace ShadowNetBackend.Features.Agents.GetAgents;

public record GetAgentsResponse(IEnumerable<AgentDto> Agents);

public class GetAgentsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/agents", async (
            [AsParameters] AgentParameters parameters,
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetAgentsQuery(parameters), cancellationToken);
            var response = new GetAgentsResponse(result);
            return TypedResults.Ok(response);
        })
        .WithName("GetAgents")
        .WithDescription("Get agents")
        .Produces<GetAgentsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
