namespace Innermost.Meet.API.Application.IntegrationEventHandles.IntegrationEvents
{
    public record UserRegisteredIntegrationEvent:IntegrationEvent
    {
        public string UserId { get; init; }
        public UserRegisteredIntegrationEvent(string userId)
        {
            UserId= userId;
        }
    }
}
