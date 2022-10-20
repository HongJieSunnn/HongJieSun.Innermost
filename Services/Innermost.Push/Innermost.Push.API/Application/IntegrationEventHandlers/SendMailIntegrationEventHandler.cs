namespace Innermost.Push.API.Application.IntegrationEventHandlers
{
    public class SendMailIntegrationEventHandler : IIntegrationEventHandler<SendMailIntegrationEvent>
    {
        private readonly ISendEmailService _sendEmailService;
        public SendMailIntegrationEventHandler(ISendEmailService sendEmailService)
        {
            _sendEmailService = sendEmailService;
        }
        public async Task Handle(SendMailIntegrationEvent @event)
        {
            await _sendEmailService.SendEmailAsync(@event.ToEmailAddress, @event.Subject, @event.Body, @event.IsHtml);
        }
    }
}
