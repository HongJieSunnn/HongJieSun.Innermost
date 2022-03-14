namespace Innermost.LogLife.API.Services.IntegrationEventServices
{
    public interface ILogLifeIntegrationEventService:IIntegrationEventService
    {
        Task PublishEventsAsync(Guid transactionId);
    }
}
