namespace ShadowNetBackend.Features.Agents.DeleteAgent;

public class DeleteAgentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/agents/{id:guid}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            await sender.Send(new DeleteAgentCommand(id), cancellationToken);
            return TypedResults.NoContent();
        })
        .WithTags("Agents")
        .WithName("DeleteAgent")
        .WithDescription("Delete agent")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
