namespace Innermost.Meet.Domain.AggregatesModels.UserSocialContactAggregate.Entities
{
    /// <summary>
    /// Confidant just is friend.Entity.Id is userId.
    /// </summary>
    public class Confidant : Entity<string>
    {
        public string ConfidantUserId { get; private set; }
        //TODO Confidant profile may not store here.We get confidant profile in Queries.
        //TODO But we still should use Confidant class.Because may add the properties like confidant level later.
        public string ChattingContextId { get; private set; }
        public DateTime CreateTime { get; private set; }
        public Confidant(string confidantUserId, string chattingContextId, DateTime createTime)
        {
            Id = ObjectId.GenerateNewId().ToString();
            ConfidantUserId = confidantUserId;
            ChattingContextId = chattingContextId;
            CreateTime = createTime;
        }
    }
}
