using ShadowNetBackend.Features.Messages.CreateMessage;
using ShadowNetBackend.Features.Messages.DeleteMessage;
using ShadowNetBackend.Features.Messages.GetAllMessages;
using ShadowNetBackend.Features.Messages.GetByIdMessage;

namespace ShadowNetBackend.Features.Messages;

public static class MessageEndpoints
{
    public static void MapMessageEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/messages").WithTags("Messages");

        group.MapGet("", GetAllMessages)
            .WithName("GetAllMessages")
            .Produces<IEnumerable<MessageResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapGet("/{id:guid}", GetMessageById)
            .WithName("GetMessageById")
            .Produces<MessageResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("", CreateMessage)
            .WithName("CreateMessage")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:guid}", DeleteMessage)
            .WithName("DeleteMessage")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetAllMessages([AsParameters] MessageParameters parameters, ISender sender, CancellationToken cancellationToken)
    {
        if (parameters is null)
            return TypedResults.BadRequest("Invalid Parameters");

        var Messages = await sender.Send(new GetMessagesQuery(parameters), cancellationToken);
        return TypedResults.Ok(Messages);
    }

    private static async Task<IResult> GetMessageById(Guid id, ISender sender, CancellationToken cancellation)
    {
        var Message = await sender.Send(new GetByIdMessageQuery(id), cancellation);
        return TypedResults.Ok(Message);
    }

    private static async Task<IResult> CreateMessage(CreateMessageCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var id = await sender.Send(command, cancellationToken);

        return TypedResults.Created($"/api/messages/{id}", id);
    }

    private static async Task<IResult> DeleteMessage(Guid id, ISender sender, CancellationToken cancellation)
    {
        await sender.Send(new DeleteMessageCommand(id), cancellation);
        return TypedResults.NoContent();
    }
}
