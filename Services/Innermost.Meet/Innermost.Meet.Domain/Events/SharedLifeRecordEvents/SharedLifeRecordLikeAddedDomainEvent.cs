namespace Innermost.Meet.Domain.Events.SharedLifeRecordEvents
{
    public class SharedLifeRecordLikeAddedDomainEvent : INotification
    {
        public string LikerUserId { get; init; }
        public DateTime LikeTime { get; init; }
        public SharedLifeRecord SharedLifeRecord { get; init; }
        public SharedLifeRecordLikeAddedDomainEvent(string likerUserId, DateTime likeTime, SharedLifeRecord sharedLifeRecord)
        {
            LikerUserId = likerUserId;
            LikeTime = likeTime;
            SharedLifeRecord = sharedLifeRecord;
        }
    }
}
