using Microsoft.AspNetCore.SignalR;

namespace ShadowNetBackend.Features.Communications;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
