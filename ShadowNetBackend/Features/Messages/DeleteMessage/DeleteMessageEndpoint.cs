namespace ShadowNetBackend.Features.Messages.DeleteMessage;

public class DeleteMessageEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/messages/{id:guid}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            await sender.Send(new DeleteMessageCommand(id), cancellationToken);
            return TypedResults.NoContent();
        })
        .WithTags("Messages")
        .WithName("DeleteMessage")
        .WithDescription("Delete message")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}