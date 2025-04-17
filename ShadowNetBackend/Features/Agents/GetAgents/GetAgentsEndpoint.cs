using ShadowNetBackend.Features.Agents.Common;
using ShadowNetBackend.Features.Agents.GetAllAgents;

namespace ShadowNetBackend.Features.Agents.GetAgents;

public class GetAgentsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/agents", async (
            [AsParameters] AgentParameters parameters,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetAgentsQuery(parameters), cancellationToken);
            return TypedResults.Ok(result);
        })
        .WithTags("Agents")
        .WithName("GetAgents")
        .WithDescription("Get agents")
        .Produces<IEnumerable<AgentDto>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}