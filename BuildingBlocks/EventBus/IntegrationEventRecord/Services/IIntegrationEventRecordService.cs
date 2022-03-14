// Copyright (c) .NET Foundation and Contributors. All Rights Reserved.
//
// Distributed under MIT license.
// See file LICENSE for detail or copy at https://opensource.org/licenses/MIT

// Framework code of microservices and domain drive design pattern
namespace IntegrationEventRecord.Services
{
    public interface IIntegrationEventRecordService
    {
        Task<IEnumerable<IntegrationEventRecordModel>> RetrieveEventsByEventContentsToPublishAsync(Guid transactionId);
        Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction);
        Task MarkEventAsInProcessAsync(Guid eventId);
        Task MarkEventAsPublishedAsync(Guid eventId);
        Task MarkEventAsPublishedFailedAsync(Guid eventId);
    }
}
