using EventBusCommon;

namespace Microsoft.Extensions.Logging
{
    public static class IntegrationEventILoggerExtensions
    {
        public static void LogIntegrationEventHandlerStartHandling<T>(this ILogger<T> logger, IntegrationEvent @event, string appName)
        {
            logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, appName, @event);
        }

        public static void LogPublishingIntegrationEvent<T>(this ILogger<T> logger, Guid integrationEventId, string? appName, IntegrationEvent @event)
        {
            logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", integrationEventId, appName, @event);
        }

        public static void LogPublishIntegrationFailedEvent<T>(this ILogger<T> logger, Exception exception, Guid integrationEventId, string? appName)
        {
            logger.LogError(exception, "ERROR publishing integration event: {IntegrationEventId} from {AppName}", integrationEventId, appName);
        }
    }
}