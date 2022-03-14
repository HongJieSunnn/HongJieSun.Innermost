namespace EventBusServiceBus
{
    public interface IServiceBusPersisterConnection : IAsyncDisposable
    {
        string ServiceBusConnectionString { get; }
        ServiceBusClient CreateModel();
        ServiceBusAdministrationClient CreateAdministrationModel();
    }
}
