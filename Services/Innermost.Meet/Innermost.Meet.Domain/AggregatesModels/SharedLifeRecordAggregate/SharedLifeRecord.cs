using Innermost.Meet.Domain.AggregatesModels.SharedLifeRecordAggregate.Entities;
using Innermost.Meet.Domain.AggregatesModels.SharedLifeRecordAggregate.ValueObjects;
using Innermost.Meet.Domain.Events.SharedLifeRecordEvents;
using TagS.Microservices.Client.DomainSeedWork;
using TagS.Microservices.Client.Models;

namespace Innermost.Meet.Domain.AggregatesModels.SharedLifeRecordAggregate
{
    public class SharedLifeRecord : TagableEntity<string>, IAggregateRoot
    {
        /// <summary>
        /// Record Id in Loglife.In other works,recordId is id in sql table.
        /// </summary>
        public int RecordId { get; init; }
        public string UserId { get; init; }
        public string UserName { get; private set; }
        public string UserNickName { get; private set; }
        public string UserAvatarUrl { get; private set; }

        public string? Title { get; private set; }
        public string Text { get; private set; }

        public Location? Location { get; private set; }

        public MusicRecord? MusicRecord { get; private set; }

        [BsonRequired]
        [BsonElement("ImagePaths")]
        private readonly List<string>? _imagePaths;
        public IReadOnlyCollection<string>? ImagePaths => _imagePaths?.AsReadOnly();

        public int LikesCount { get; private set; }

        [BsonRequired]
        [BsonElement("Likes")]
        private readonly List<Like> _likes;
        public IReadOnlyCollection<Like> Likes => _likes.AsReadOnly();

        public DateTime CreateTime { get; private set; }
        public DateTime? UpdateTime { get; private set; }
        public DateTime? DeleteTime { get; private set; }
        public SharedLifeRecord(
            string? objectId, int recordId, string userId,string userName,string userNickName,string userAvatarUrl,
            string? title, string text,
            Location? location, MusicRecord? musicRecord,
            List<string>? imagePaths, int likesCount, List<Like>? likes, List<TagSummary> tagSummaries,
            DateTime createTime, DateTime? updateTime, DateTime? deleteTime
        ) : base(tagSummaries)
        {
            Id = objectId;
            RecordId = recordId;
            UserId = userId;
            UserName = userName;
            UserNickName = userNickName;
            UserAvatarUrl = userAvatarUrl;
            Title = title;
            Text = text;
            Location = location;
            MusicRecord = musicRecord;
            _imagePaths = imagePaths;
            LikesCount = likesCount;
            _likes = likes ?? new List<Like>();
            CreateTime = createTime;
            UpdateTime = updateTime;
            DeleteTime = deleteTime;
        }

        public UpdateDefinition<SharedLifeRecord> AddLike(Like like)
        {
            ++LikesCount;
            _likes.Add(like);
            AddDomainEvent(new SharedLifeRecordLikeAddedDomainEvent(like.LikerUserId, like.LikeTime, this));
            return Builders<SharedLifeRecord>.Update.Inc(l => l.LikesCount, 1).AddToSet("Likes", like);
        }

        public UpdateDefinition<SharedLifeRecord> SetDeleted()
        {
            DeleteTime = DateTime.Now;
            return Builders<SharedLifeRecord>.Update.Set(t => t.DeleteTime, DeleteTime);
        }

        protected override IReferrer ToReferrer()
        {
            throw new NotImplementedException();
        }
    }
}
