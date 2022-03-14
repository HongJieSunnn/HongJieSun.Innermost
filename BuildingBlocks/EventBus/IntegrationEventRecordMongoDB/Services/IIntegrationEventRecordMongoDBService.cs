namespace IntegrationEventRecordMongoDB.Services
{
    public interface IIntegrationEventRecordMongoDBService
    {
        Task SaveEventAsync(IntegrationEvent @event);
        Task MarkEventAsInProcessAsync(Guid eventId);
        Task MarkEventAsPublishedAsync(Guid eventId);
        Task MarkEventAsPublishedFailedAsync(Guid eventId);
        Task<IEnumerable<IntegrationEventRecordMongoDBModel>> GetIntegrationEventRecords(IEnumerable<Guid> eventIds);
    }
}
