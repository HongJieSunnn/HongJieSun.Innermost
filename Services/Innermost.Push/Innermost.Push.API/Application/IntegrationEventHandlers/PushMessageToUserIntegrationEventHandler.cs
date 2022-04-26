namespace Innermost.Push.API.Application.IntegrationEventHandlers
{
    public class PushMessageToUserIntegrationEventHandler : IIntegrationEventHandler<PushMessageToUserIntegrationEvent>
    {
        private readonly IHubContext<PushHub> _hubContext;
        public PushMessageToUserIntegrationEventHandler(IHubContext<PushHub> hubContext)
        {
            _hubContext=hubContext;
        }
        public async Task Handle(PushMessageToUserIntegrationEvent @event)
        {
            await _hubContext.Clients.User(@event.UserId).SendAsync($"Push{@event.Type}Message",@event.Message);
        }
    }
}
