using ILoggerExtensions;
using MongoDBExtensions;

namespace IntegrationEventServiceMongoDB.Services
{
    public class CommonIntegrationEventServiceMongoDB : IIntegrationEventService
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IClientSessionHandle? _session;
        private readonly IMongoCollection<IntegrationEventMongoDBModel> _integrationEvents;

        private readonly IAsyncEventBus _eventBus;
        private readonly ILogger<CommonIntegrationEventServiceMongoDB> _logger;
        public CommonIntegrationEventServiceMongoDB(IMongoDatabase mongoDatabase, IAsyncEventBus eventBus, ILogger<CommonIntegrationEventServiceMongoDB> logger, IClientSessionHandle? session = null)
        {
            _mongoDatabase = mongoDatabase;
            _eventBus = eventBus;
            _logger = logger;
            _session = session;

            _integrationEvents = _mongoDatabase.GetCollection<IntegrationEventMongoDBModel>("IntegrationEventRecordCollection");
            if (!_integrationEvents.Indexes.List().Any())
            {
                var sessionIdIndex = Builders<IntegrationEventMongoDBModel>.IndexKeys.Ascending(e => e.SessionId);

                _integrationEvents.Indexes.CreateOne(new CreateIndexModel<IntegrationEventMongoDBModel>(sessionIdIndex));
            }
        }

        public Task SaveEventAsync(IntegrationEvent @event)
        {
            var eventModel = new IntegrationEventMongoDBModel(@event.Id, nameof(@event), @event, @event.CreationDate, _session?.GetSessionId());
            return _integrationEvents.InsertOneAsync(_session, eventModel);
        }
        protected Task MarkEventAsInProcessAsync(Guid eventId)
        {
            var @event = _integrationEvents.Find(e => e.EventId == eventId).First();
            var update = Builders<IntegrationEventMongoDBModel>
                .Update
                .Set(e => e.EventState, EventState.InProcess)
                .Set(e => e.TimeSend, @event.TimeSend + 1);
            return _integrationEvents.UpdateOneAsync(_session, e => e.EventId == eventId, update);
        }

        protected Task MarkEventAsPublishedAsync(Guid eventId)
        {
            var update = Builders<IntegrationEventMongoDBModel>
                .Update
                .Set(e => e.EventState, EventState.Published);
            return _integrationEvents.UpdateOneAsync(_session, e => e.EventId == eventId, update);
        }

        protected Task MarkEventAsPublishedFailedAsync(Guid eventId)
        {
            var update = Builders<IntegrationEventMongoDBModel>
                .Update
                .Set(e => e.EventState, EventState.PublishedFailed);
            return _integrationEvents.UpdateOneAsync(_session, e => e.EventId == eventId, update);
        }

        public async Task PublishEventsAsync(IEnumerable<Guid> eventIds)
        {
            var recordFilter = Builders<IntegrationEventMongoDBModel>.Filter.In("_id", eventIds.Select(e=>e.ToString()));
            var records = await _integrationEvents.Find(recordFilter).ToListAsync();

            foreach (var record in records)
            {
                _logger.LogPublishingIntegrationEvent(record.EventId, Assembly.GetEntryAssembly()?.FullName, record.EventContent);

                try
                {
                    await MarkEventAsInProcessAsync(record.EventId);
                    await _eventBus.Publish(record.EventContent);
                    await MarkEventAsPublishedAsync(record.EventId);
                }
                catch (Exception ex)
                {
                    _logger.LogPublishIntegrationFailedEvent(ex, record.EventId, Assembly.GetEntryAssembly()?.FullName);
                    throw;
                }
            }
        }

        public async Task PublishEventsAsync(string sessionId)
        {
            var recordFilter = Builders<IntegrationEventMongoDBModel>.Filter.Eq(r => r.SessionId, sessionId);
            var records = await _integrationEvents.Find(recordFilter).ToListAsync();

            foreach (var record in records)
            {
                _logger.LogPublishingIntegrationEvent(record.EventId, Assembly.GetEntryAssembly()?.FullName, record.EventContent);

                try
                {
                    await MarkEventAsInProcessAsync(record.EventId);
                    await _eventBus.Publish(record.EventContent);
                    await MarkEventAsPublishedAsync(record.EventId);
                }
                catch (Exception ex)
                {
                    _logger.LogPublishIntegrationFailedEvent(ex, record.EventId, Assembly.GetEntryAssembly()?.FullName);
                    throw;
                }
            }
        }
    }
}
