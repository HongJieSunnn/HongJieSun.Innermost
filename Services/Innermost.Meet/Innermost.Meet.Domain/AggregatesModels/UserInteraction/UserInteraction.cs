using Innermost.Meet.Domain.AggregatesModels.UserInteraction.Entities;

namespace Innermost.Meet.Domain.AggregatesModels.UserInteraction
{
    public class UserInteraction : Entity<string>, IAggregateRoot
    {
        public string UserId { get; init; }

        [BsonRequired]
        [BsonElement("UserLikes")]
        private readonly List<UserLike> _userLikes;
        public IReadOnlyCollection<UserLike> UserLikes => _userLikes.AsReadOnly();
        public UserInteraction(string? id,string userId,List<UserLike> userLikes)
        {
            Id= id;
            UserId= userId;
            _userLikes= userLikes;
        }
    }
}
