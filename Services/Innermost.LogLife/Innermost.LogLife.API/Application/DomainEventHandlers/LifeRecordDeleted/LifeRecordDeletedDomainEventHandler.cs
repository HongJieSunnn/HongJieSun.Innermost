namespace Innermost.LogLife.API.Application.DomainEventHandlers.LifeRecordDeleted
{
    public class LifeRecordDeletedDomainEventHandler : INotificationHandler<LifeRecordDeletedDomainEvent>
    {
        private readonly ILogger<LifeRecordDeletedDomainEventHandler> _logger;
        private readonly ILogLifeIntegrationEventService _logLifeIntegrationEventService;
        public LifeRecordDeletedDomainEventHandler(ILogger<LifeRecordDeletedDomainEventHandler> logger, ILogLifeIntegrationEventService logLifeIntegrationEventService)
        {
            _logger = logger;
            _logLifeIntegrationEventService = logLifeIntegrationEventService;

        }
        public async Task Handle(LifeRecordDeletedDomainEvent notification, CancellationToken cancellationToken)
        {
            var deletedIntegrationEvent = new LifeRecordDeletedIntegrationEvent(notification.RecordId, notification.UserId);
            await _logLifeIntegrationEventService.SaveEventAsync(deletedIntegrationEvent);
        }
    }
}
