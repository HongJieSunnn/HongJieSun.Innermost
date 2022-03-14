namespace Innermost.LogLife.API.Application.IntegrationEvents
{
    public record LifeRecordDeletedIntegrationEvent:IntegrationEvent
    {
        public int RecordId { get;private set; }
        public string UserId { get; private set; }
        public LifeRecordDeletedIntegrationEvent(int recordId,string userId)
        {
            RecordId = recordId;
            UserId = userId;
        }
    }
}
