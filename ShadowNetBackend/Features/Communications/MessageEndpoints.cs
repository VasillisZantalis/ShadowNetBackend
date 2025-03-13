using ShadowNetBackend.Features.Communications.CreateMessage;
using ShadowNetBackend.Features.Communications.DeleteMessage;
using ShadowNetBackend.Features.Communications.GetAllMessages;
using ShadowNetBackend.Features.Communications.GetByIdMessage;

namespace ShadowNetBackend.Features.Communications;

public static class MessageEndpoints
{
    public static void MapMessageEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/messages");

        group.MapGet("", GetAllMessages).WithName("GetAllMessages");
        group.MapGet("/{id:guid}", GetMessageById).WithName("GetMessageById");
        group.MapPost("", CreateMessage).WithName("CreateMessage");
        group.MapDelete("/{id:guid}", DeleteMessage).WithName("DeleteMessage");
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
