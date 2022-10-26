namespace Innermost.Identity.API.IntegrationEvents
{
    public record UserRegisteredIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; init; }
        public UserRegisteredIntegrationEvent(string userId)
        {
            UserId = userId;
        }
    }
}
