namespace Innermost.LogLife.Domain.Events.LifeRecord
{
    public class LifeRecordDeletedDomainEvent:INotification
    {
        public int RecordId { get; private set; }
        public string UserId { get; private set; }

        public LifeRecordDeletedDomainEvent(int recordId,string userId)
        {
            RecordId = recordId;
            UserId = userId;
        }
    }
}
