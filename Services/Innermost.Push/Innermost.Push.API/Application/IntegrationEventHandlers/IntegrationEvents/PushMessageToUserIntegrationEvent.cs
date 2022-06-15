namespace Innermost.Push.API.Application.IntegrationEventHandlers.IntegrationEvents
{
    public record PushMessageToUserIntegrationEvent:IntegrationEvent
    {
        public string UserId { get; set; }
        public object Message { get; set; }
        public string Type { get; set; }
        public PushMessageToUserIntegrationEvent(string userId,string message,string type)
        {
            UserId = userId;
            Message = message;
            Type = type;
        }
    }
}
