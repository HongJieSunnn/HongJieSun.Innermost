using Innermost.Meet.Domain.AggregatesModels.UserInteractionAggregate.Entities;

namespace Innermost.Meet.Domain.AggregatesModels.UserInteraction
{
    /// <summary>
    /// UserInteraction represent for interaction between user and sharedLifeRecords or more later.
    /// </summary>
    public class UserInteraction : Entity<string>, IAggregateRoot
    {
        public string UserId { get; init; }

        [BsonRequired]
        [BsonElement("RecordLikes")]
        private readonly List<RecordLike> _recordLikes;
        public IReadOnlyCollection<RecordLike> RecordLikes => _recordLikes.AsReadOnly();
        public UserInteraction(string? id, string userId, List<RecordLike> recordLikes)
        {
            Id = id;
            UserId = userId;
            _recordLikes = recordLikes;
        }

        public UpdateDefinition<UserInteraction> AddRecordLike(RecordLike recordLike)
        {
            _recordLikes.Add(recordLike);
            return Builders<UserInteraction>.Update.AddToSet("RecordLikes", recordLike);
        }
    }
}
