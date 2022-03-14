namespace EventBusCommon.Abstractions
{
    public interface IIntegrationEventService
    {
        Task AddAndSaveEventAsync(IntegrationEvent integrationEvent);
    }
}
