namespace Innermost.Meet.Domain.AggregatesModels.UserSocialContactAggregate.Entities
{
    /// <summary>
    /// Confidant just is friend.Entity.Id is userId.
    /// </summary>
    public class Confidant:Entity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public override string? Id { get => base.Id; set => base.Id = value; }
    }
}
