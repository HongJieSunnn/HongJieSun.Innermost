namespace ILoggerExtensions
{
    public static class CommandILoggerExtensions
    {
        public static void LogSendCommand<T>(this ILogger<T> logger,string requestId,string commandName,string commandIdProperty,string commandId,object command)
        {
            logger.LogInformation(
                "----- (RequestId:{RequestId}) Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    requestId,commandName, commandIdProperty, commandId, command);
        }
    }
}
