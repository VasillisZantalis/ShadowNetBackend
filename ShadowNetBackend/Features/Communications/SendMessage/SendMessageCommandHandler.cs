using Microsoft.AspNetCore.SignalR;

namespace ShadowNetBackend.Features.Communications.SendMessage
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Unit>
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public SendMessageCommandHandler(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task<Unit> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", request.User, request.Message);
            return Unit.Value;
        }
    }

}
