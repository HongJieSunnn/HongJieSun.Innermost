using EventBusCommon;

namespace ILoggerExtensions
{
    public static class IntegrationEventILoggerExtensions
    {
        public static void LogIntegrationEventHandlerStartHandling<T>(this ILogger<T> logger, IntegrationEvent @event, string appName)
        {
            logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, appName, @event);
        }
    }
}