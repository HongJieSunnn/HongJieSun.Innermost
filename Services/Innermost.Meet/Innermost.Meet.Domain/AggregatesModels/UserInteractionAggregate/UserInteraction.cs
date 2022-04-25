using Innermost.Meet.Domain.AggregatesModels.UserInteractionAggregate.Entities;

namespace Innermost.Meet.Domain.AggregatesModels.UserInteraction
{
    /// <summary>
    /// UserInteraction represent for interaction between user and sharedLifeRecords or more later.Id is userId.
    /// </summary>
    public class UserInteraction : Entity<string>, IAggregateRoot
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public override string? Id { get => base.Id; set => base.Id = value; }

        [BsonRequired]
        [BsonElement("RecordLikes")]
        private readonly List<RecordLike> _recordLikes;
        public IReadOnlyCollection<RecordLike> RecordLikes => _recordLikes.AsReadOnly();
        public UserInteraction(string userId, List<RecordLike>? recordLikes)
        {
            Id = userId;
            _recordLikes = recordLikes??new List<RecordLike>();
        }

        public UpdateDefinition<UserInteraction> AddRecordLike(RecordLike recordLike)
        {
            _recordLikes.Add(recordLike);
            return Builders<UserInteraction>.Update.AddToSet("RecordLikes", recordLike);
        }
    }
}
