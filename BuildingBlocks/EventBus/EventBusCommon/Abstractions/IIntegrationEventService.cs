namespace EventBusCommon.Abstractions
{
    public interface IIntegrationEventService
    {
        Task SaveEventAsync(IntegrationEvent @event);
        Task PublishEventsAsync(IEnumerable<Guid> eventIds);
        Task PublishEventsAsync(string transactionOrSessionId);
    }
}
