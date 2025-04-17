using ShadowNetBackend.Features.Messages.GetByIdMessage;

namespace ShadowNetBackend.Features.Messages.GetMessageById;

public class GetMessageByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/messages/{id:guid}", async (
            Guid id,
            [FromQuery] EncryptionType? encryptionType,
            [FromQuery] string? encryptionKey,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(new GetMessageByIdQuery(id), cancellationToken);
            return TypedResults.Ok(response);
        })
        .WithTags("Messages")
        .WithName("GetMessageById")
        .WithDescription("Get message by Id")
        .Produces<MessageDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}