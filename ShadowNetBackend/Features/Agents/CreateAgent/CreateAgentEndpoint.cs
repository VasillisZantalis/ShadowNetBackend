namespace ShadowNetBackend.Features.Agents.CreateAgent;

public class CreateAgentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/agents", async (
            [FromBody] AgentForCreationDto agentForCreation,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new CreateAgentCommand(agentForCreation), cancellationToken);
            return TypedResults.Created($"/api/agents/{result}", result);
        })
        .WithName("CreateAgent")
        .WithDescription("Create a new agent")
        .Produces<AgentForCreationDto>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}