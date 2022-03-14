namespace EventBusCommon.Abstractions
{
    public interface IAsyncEventBus
    {
        Task Publish(IntegrationEvent @event);

        Task Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        Task UnSubsribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler;
    }
}
