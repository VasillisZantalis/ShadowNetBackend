namespace ShadowNetBackend.Features.Agents.CreateAgent;

public record CreateAgentRequest(AgentForCreationDto AgentForCreation);
public record CreateAgentResponse(Guid Id);

public class CreateAgentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/agents", async (
            [FromBody] CreateAgentRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new CreateAgentCommand(request.AgentForCreation), cancellationToken);
            var response = new CreateAgentResponse(result.Id);
            return TypedResults.Created($"/api/agents/{response.Id}", response);
        })
        .WithName("CreateAgent")
        .WithDescription("Create a new agent")
        .Produces<CreateAgentResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}