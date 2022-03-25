namespace IntegrationEventRecordMongoDB.Services
{
    public class IntegrationEventRecordMongoDBService : IIntegrationEventRecordMongoDBService
    {
        /// <summary>
        /// We always store integrationEventRecord in the same database of the service.
        /// </summary>
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IClientSessionHandle _session;
        private readonly IMongoCollection<IntegrationEventRecordMongoDBModel> _integrationEvents;
        public IntegrationEventRecordMongoDBService(IMongoDatabase database, IClientSessionHandle session)
        {
            _mongoDatabase = database;
            _integrationEvents = _mongoDatabase.GetCollection<IntegrationEventRecordMongoDBModel>("IntegrationEventRecordCollection");

            //Create indexes while indexes has not been created.
            if (!_integrationEvents.Indexes.List().Any())
            {
                var guidIndex = Builders<IntegrationEventRecordMongoDBModel>.IndexKeys.Ascending(e => e.EventId);

                _integrationEvents.Indexes.CreateOne(new CreateIndexModel<IntegrationEventRecordMongoDBModel>(guidIndex));
            }
            _session = session;
        }

        public async Task<IEnumerable<IntegrationEventRecordMongoDBModel>> GetIntegrationEventRecords(IEnumerable<Guid> eventIds)
        {
            var recordCursor= await _integrationEvents.FindAsync(_session,e => eventIds.Contains(e.EventId));
            return recordCursor.ToList();
        }

        public Task MarkEventAsInProcessAsync(Guid eventId)
        {
            var @event = _integrationEvents.Find(e => e.EventId == eventId).First();
            var update = Builders<IntegrationEventRecordMongoDBModel>
                .Update
                .Set(e => e.EventState,EventState.InProcess)
                .Set(e => e.TimeSend,@event.TimeSend+1);
            return _integrationEvents.UpdateOneAsync(_session, e => e.EventId == eventId, update);
        }

        public Task MarkEventAsPublishedAsync(Guid eventId)
        {
            var update = Builders<IntegrationEventRecordMongoDBModel>
                .Update
                .Set(e => e.EventState, EventState.Published);
            return _integrationEvents.UpdateOneAsync(_session, e => e.EventId == eventId, update);
        }

        public Task MarkEventAsPublishedFailedAsync(Guid eventId)
        {
            var update = Builders<IntegrationEventRecordMongoDBModel>
                .Update
                .Set(e => e.EventState, EventState.PublishedFailed);
            return _integrationEvents.UpdateOneAsync(_session, e => e.EventId == eventId, update);
        }

        public Task SaveEventAsync(IntegrationEvent @event)
        {
            var eventModel = new IntegrationEventRecordMongoDBModel(@event.Id, nameof(@event), @event, @event.CreationDate);
            return _integrationEvents.InsertOneAsync(_session, eventModel);
        }
    }
}
