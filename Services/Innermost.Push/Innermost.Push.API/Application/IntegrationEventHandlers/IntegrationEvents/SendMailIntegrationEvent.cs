namespace Innermost.Push.API.Application.IntegrationEventHandlers.IntegrationEvents
{
    public record SendMailIntegrationEvent : IntegrationEvent
    {
        public string ToEmailAddress { get; init; }
        public string Subject { get; init; }
        public string Body { get; init; }
        public bool IsHtml { get; init; }
        public SendMailIntegrationEvent(string toEmailAddress, string subject, string body, bool ishtml = false)
        {
            ToEmailAddress = toEmailAddress;
            Subject = subject;
            Body = body;
            IsHtml = ishtml;
        }
    }
}
