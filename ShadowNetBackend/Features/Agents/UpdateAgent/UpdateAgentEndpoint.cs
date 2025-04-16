namespace ShadowNetBackend.Features.Agents.UpdateAgent;

public record UpdateAgentRequest(AgentForUpdateDto AgentForUpdate);

public class UpdateAgentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/agents", async (
            [FromBody] UpdateAgentRequest request,
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
        {
            await sender.Send(new UpdateAgentCommand(request.AgentForUpdate), cancellationToken);
            return TypedResults.NoContent();
        })
        .WithName("UpdateAgent")
        .WithDescription("Update an existing agent")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
