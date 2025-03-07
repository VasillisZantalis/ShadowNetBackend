using MediatR;
using ShadowNetBackend.Features.Agents.CreateAgent;
using ShadowNetBackend.Features.Agents.DeleteAgent;
using ShadowNetBackend.Features.Agents.GetAllAgents;
using ShadowNetBackend.Features.Agents.GetByIdAgent;
using ShadowNetBackend.Features.Agents.UpdateAgent;

namespace ShadowNetBackend.Features.Agents;

public static class AgentEndpoints
{
    public static void MapAgentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/agents");

        group.MapGet("", GetAllAgents).WithName("GetAllAgents");
        group.MapGet("/{id:guid}", GetAgentById).WithName("GetAgentById");
        group.MapPost("", CreateAgent).WithName("CreateAgent");
        group.MapPut("/{id:guid}", UpdateAgent).WithName("UpdateAgent");
        group.MapDelete("/{id:guid}", DeleteAgent).WithName("DeleteAgent");
    }

    private static async Task<IResult> GetAllAgents([AsParameters] AgentParameters parameters, ISender sender, CancellationToken cancellationToken)
    {
        if (parameters is null)
        {
            return TypedResults.BadRequest("Invalid parameters");
        }

        var agents = await sender.Send(new GetAgentsQuery(parameters), cancellationToken);

        return TypedResults.Ok(agents);
    }

    private static async Task<IResult> GetAgentById(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        var agent = await sender.Send(new GetByIdAgentQuery(id), cancellationToken);

        return TypedResults.Ok(agent);
    }

    private static async Task<IResult> CreateAgent(CreateAgentCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var id = await sender.Send(command, cancellationToken);

        return id is null
            ? TypedResults.BadRequest("Failed to create agent")
            : TypedResults.Created($"/api/agents/{id}", id);
    }

    private static async Task<IResult> UpdateAgent(Guid id, UpdateAgentCommand command, ISender sender, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return TypedResults.BadRequest("Id mismatch");

        var success = await sender.Send(command, cancellationToken);

        return !success
            ? TypedResults.BadRequest("Failed to update agent")
            : TypedResults.NoContent();
    }

    private static async Task<IResult> DeleteAgent(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteAgentCommand(id), cancellationToken);

        return TypedResults.NoContent();
    }
}
