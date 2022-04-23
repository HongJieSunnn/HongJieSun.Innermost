global using EventBusCommon;

namespace Innermost.Meet.SignalRHub.Application.IntegrationEventHandlers.IntegrationEvents
{
    public record AdminSendMessageToUserIntegrationEvent:IntegrationEvent
    {
        public string ToUserId { get; init; }
        public string Message { get; set; }
        public DateTime CreateTime { get; set; }
        public AdminSendMessageToUserIntegrationEvent(string toUserId,string message)
        {
            ToUserId=toUserId;
            Message=message;
            CreateTime=DateTime.Now;
        }
    }
}
