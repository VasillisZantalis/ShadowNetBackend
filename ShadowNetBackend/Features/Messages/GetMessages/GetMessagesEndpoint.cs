using ShadowNetBackend.Features.Messages.GetAllMessages;

namespace ShadowNetBackend.Features.Messages.GetMessages;

public class GetMessagesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/messages", async (
            [AsParameters] MessageParameters parameters,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetMessagesQuery(parameters), cancellationToken);
            return TypedResults.Ok(result);
        })
        .WithTags("Messages")
        .WithName("GetMessages")
        .WithDescription("Get messages")
        .Produces<IEnumerable<MessageDto>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}