using Innermost.Meet.Domain.AggregatesModels.UserInteractionAggregate.Entities;

namespace Innermost.Meet.Domain.AggregatesModels.UserInteractionAggregate
{
    /// <summary>
    /// UserInteraction represent for interaction between user and sharedLifeRecords or more later.
    /// </summary>
    public class UserInteraction : Entity<string>, IAggregateRoot
    {
        public string UserId { get; private set; }
        [BsonRequired]
        [BsonElement("RecordLikes")]
        private List<RecordLike> _recordLikes;
        public IReadOnlyCollection<RecordLike> RecordLikes => _recordLikes.AsReadOnly();
        public UserInteraction(string userId, List<RecordLike>? recordLikes)
        {
            UserId = userId;
            _recordLikes = recordLikes ?? new List<RecordLike>();
        }

        public UpdateDefinition<UserInteraction> AddRecordLike(RecordLike recordLike)
        {
            _recordLikes.Add(recordLike);
            return Builders<UserInteraction>.Update.AddToSet("RecordLikes", recordLike);
        }
    }
}
