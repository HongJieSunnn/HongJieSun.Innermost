namespace EventBusCommon.Abstractions
{
    public interface IIntegrationEventService//TODO shall I create interface ISQLIntegrationEventService used in TransactionBehavior of LogLife or maybe not because I will not use SQL except LogLife.
    {
        Task AddAndSaveEventAsync(IntegrationEvent integrationEvent);
    }
}
