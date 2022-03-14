namespace Innermost.LogLife.API.Application.DomainEventHandlers.LifeRecordSetShared
{
    public class LifeRecordSetSharedDomainEventHandler : INotificationHandler<LifeRecordSetSharedDomainEvent>
    {
        private readonly ILogger<LifeRecordSetSharedDomainEventHandler> _logger;
        private readonly ILogLifeIntegrationEventService _logLifeIntegrationEventService;
        public LifeRecordSetSharedDomainEventHandler(ILogger<LifeRecordSetSharedDomainEventHandler> logger,ILogLifeIntegrationEventService logLifeIntegrationEventService)
        {
            _logger=logger;
            _logLifeIntegrationEventService=logLifeIntegrationEventService;
        }
        public async Task Handle(LifeRecordSetSharedDomainEvent notification, CancellationToken cancellationToken)
        {
            var setSharedIntegrationEvent = new LifeRecordSetSharedIntegrationEvent(
                notification.RecordId, notification.UserId, notification.Title, notification.Text,
                notification.LocationUId, notification.LocationName, notification.Province, notification.City, notification.District, notification.Address, notification.Longitude, notification.Latitude,
                notification.MusicId, notification.MusicName, notification.Singer, notification.Album,
                notification.ImagePaths,
                notification.CreateTime, notification.UpdateTime, notification.DeleteTime,
                notification.TagSummaries
            );
            await _logLifeIntegrationEventService.AddAndSaveEventAsync(setSharedIntegrationEvent);
        }
    }
}
