using ShadowNetBackend.Features.Agents.Common;
using ShadowNetBackend.Features.Agents.CreateAgent;
using ShadowNetBackend.Features.Agents.DeleteAgent;
using ShadowNetBackend.Features.Agents.GetAllAgents;
using ShadowNetBackend.Features.Agents.GetByIdAgent;
using ShadowNetBackend.Features.Agents.UpdateAgent;
using ShadowNetBackend.Features.Criminals.Common;

namespace ShadowNetBackend.Features.Agents;

public static class AgentEndpoints
{
    public static void MapAgentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/agents").WithTags("Agents");

        group.MapGet("", GetAllAgents)
            .WithName("GetAllAgents")
            .Produces<IEnumerable<AgentResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapGet("/{id:guid}", GetAgentById)
            .WithName("GetAgentById")
            .Produces<AgentResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("", CreateAgent)
            .WithName("CreateAgent")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id:guid}", UpdateAgent)
            .WithName("UpdateAgent")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:guid}", DeleteAgent)
            .WithName("DeleteAgent")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
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
