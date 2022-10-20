using Innermost.Meet.Domain.AggregatesModels.UserChattingAggregate.Entities;

namespace Innermost.Meet.Domain.AggregatesModels.UserChattingAggregate
{
    /// <summary>
    /// UserChatting AggregateRoot.
    /// </summary>
    /// <remarks>
    /// Actually,add List<ChattingRecord> _chattingRecords in UserSocialContact.Confidant is fine too.But should add to each other.
    /// </remarks>
    public class UserChattingContext : Entity<string>, IAggregateRoot
    {
        [BsonRequired]
        [BsonElement("Users")]
        private string[] _users;
        public IReadOnlyCollection<string> Users => _users;

        [BsonRequired]
        [BsonElement("ChattingRecords")]

        private List<ChattingRecord> _chattingRecords;
        public IReadOnlyCollection<ChattingRecord> ChattingRecords => _chattingRecords;

        public UserChattingContext(string id, string userId1, string userId2, List<ChattingRecord>? chattingRecords)
        {
            Id = id;
            _users = new string[2] { userId1, userId2 }.OrderBy(s => s).ToArray();
            _chattingRecords = chattingRecords ?? new List<ChattingRecord>();
        }

        public UserChattingContext(string id, string[] users, List<ChattingRecord>? chattingRecords)
        {
            Id = id;
            _users = users.Length == 2 ? users.OrderBy(s => s).ToArray() : throw new ArgumentException("UserChattingContext's users must be only two.");
            _chattingRecords = chattingRecords ?? new List<ChattingRecord>();
        }

        public UpdateDefinition<UserChattingContext> AddManyChattingRecords(IEnumerable<ChattingRecord> chattingRecords)
        {
            _chattingRecords.AddRange(chattingRecords);

            return Builders<UserChattingContext>.Update.AddToSetEach("ChattingRecords", chattingRecords);
        }
    }
}
