using EventBusCommon.Abstractions;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace IntegrationEventServiceSQL.Services
{
    public class CommonIntegrationEventServiceSQL<TDbContext> : IIntegrationEventService where TDbContext : DbContext
    {
        private readonly TDbContext _context;
        private readonly List<Type> _eventTypes;

        private readonly IAsyncEventBus _eventBus;
        private readonly ILogger<CommonIntegrationEventServiceSQL<TDbContext>> _logger;
        public CommonIntegrationEventServiceSQL(TDbContext context, IAsyncEventBus eventBus, ILogger<CommonIntegrationEventServiceSQL<TDbContext>> logger)
        {
            _context = context;
            _eventBus = eventBus;
            _logger = logger;

            var entryAssembly = Assembly.GetEntryAssembly();
            var tagClientAssembly = entryAssembly!.GetReferencedAssemblies().FirstOrDefault(a => a.Name == "TagS.Microservices.Client");
            _eventTypes = Assembly.Load(entryAssembly.FullName!)
                .GetTypes()
                .Where(t => t.BaseType == typeof(IntegrationEvent))
                .ToList();

            if (tagClientAssembly is not null)
            {
                _eventTypes.AddRange(
                    Assembly.Load(tagClientAssembly.FullName)
                    .GetTypes()
                    .Where(t => t.BaseType == typeof(IntegrationEvent))
                );
            }
        }
        public Task SaveEventAsync(IntegrationEvent @event)
        {
            var transactionId = _context.Database.CurrentTransaction?.TransactionId;

            var eventRecord = new IntegrationEventSQLModel(@event, transactionId);

            _context.Add(eventRecord);

            return _context.SaveChangesAsync();
        }

        public Task MarkEventAsInProcessAsync(Guid eventId)
        {
            return UpdateEventState(eventId, EventState.InProcess);
        }

        public Task MarkEventAsPublishedAsync(Guid eventId)
        {
            return UpdateEventState(eventId, EventState.Published);
        }

        public Task MarkEventAsPublishedFailedAsync(Guid eventId)
        {
            return UpdateEventState(eventId, EventState.PublishedFailed);
        }

        private Task UpdateEventState(Guid eventId, EventState state)
        {
            var eventRecord = _context.Find<IntegrationEventSQLModel>(eventId);

            if (eventRecord is null)
                throw new InvalidOperationException($"Can not get integration event with eventId:{eventId}");

            if (eventRecord.State == EventState.InProcess)
                eventRecord.TimesSend++;

            eventRecord.State = state;

            return _context.SaveChangesAsync();
        }

        public async Task PublishEventsAsync(IEnumerable<Guid> eventIds)
        {
            var transactionIdStr = _context.Database.CurrentTransaction?.TransactionId.ToString();

            //var records = await _context.FindAllAsync<IntegrationEventSQLModel>(eventIds);
            var records = await _context.Set<IntegrationEventSQLModel>().Where(i => eventIds.Contains(i.EventId)).ToListAsync();

            var recordsToPublish = records.OrderBy(i => i.CreateTime).Select(i => i.DeserializeIntegrationEventFromEventContent(_eventTypes.First(t => t.Name == i.EventTypeShortName)));

            foreach (var record in recordsToPublish)
            {
                _logger.LogPublishingIntegrationEvent(record.EventId, Assembly.GetEntryAssembly()?.FullName, record.IntegrationEvent ?? throw new NullReferenceException(nameof(record.IntegrationEvent)));

                try
                {
                    await MarkEventAsInProcessAsync(record.EventId);
                    await _eventBus.Publish(record.IntegrationEvent ?? throw new ArgumentNullException($"Integration event in IntegrationEventRecord with eventId({record.EventId}) is null"));
                    await MarkEventAsPublishedAsync(record.EventId);
                }
                catch (Exception ex)
                {
                    _logger.LogPublishIntegrationFailedEvent(ex, record.EventId, Assembly.GetEntryAssembly()?.FullName);
                    throw;
                }
            }
        }

        public async Task PublishEventsAsync(string transactionId)
        {
            var transactionIdStr = _context.Database.CurrentTransaction?.TransactionId.ToString();

            //var records = await _context.FindAllAsync<IntegrationEventSQLModel>(eventIds);
            var records = await _context.Set<IntegrationEventSQLModel>().Where(i => i.TransactionId == transactionId).ToListAsync();

            var recordsToPublish = records.OrderBy(i => i.CreateTime).Select(i => i.DeserializeIntegrationEventFromEventContent(_eventTypes.First(t => t.Name == i.EventTypeShortName)));

            foreach (var record in recordsToPublish)
            {
                _logger.LogPublishingIntegrationEvent(record.EventId, Assembly.GetEntryAssembly()?.FullName, record.IntegrationEvent ?? throw new NullReferenceException(nameof(record.IntegrationEvent)));

                try
                {
                    await MarkEventAsInProcessAsync(record.EventId);
                    await _eventBus.Publish(record.IntegrationEvent ?? throw new ArgumentNullException($"Integration event in IntegrationEventRecord with eventId({record.EventId}) is null"));
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
