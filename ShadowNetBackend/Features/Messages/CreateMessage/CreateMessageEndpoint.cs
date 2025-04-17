namespace ShadowNetBackend.Features.Messages.CreateMessage;

public class CreateMessageEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/messages", async (
                [FromBody] MessageForCreationDto messageForCreation,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new CreateMessageCommand(messageForCreation), cancellationToken);
                return TypedResults.Created($"/api/messages/{result}", result);
            })
            .WithName("CreateMessage")
            .WithDescription("Create a new message")
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}