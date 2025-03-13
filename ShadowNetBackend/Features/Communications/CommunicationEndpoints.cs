using ShadowNetBackend.Features.Communications.SendMessage;

namespace ShadowNetBackend.Features.Communications;

public static class CommunicationEndpoints
{
    public static void MapCommunicationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/communications");

        app.MapPost("/send-message", async (SendMessageCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(command);
            return Results.Ok();
        });
    }
}
