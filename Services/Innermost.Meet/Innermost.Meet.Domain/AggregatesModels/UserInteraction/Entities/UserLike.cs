namespace Innermost.Meet.Domain.AggregatesModels.UserInteraction.Entities
{
    /// <summary>
    /// User like contains the liked record's summary.
    /// Id is sharedRecordObjectId.
    /// </summary>
    public class UserLike : Entity<string>
    {
        public string UserId { get; init; }
        public string UserName { get; private set; }
        public string UserAvatarUrl {get;private set;}
        public string? RecordTitle { get; private set; }
        public string RecordText { get; private set; }
        public string? RecordMusicRecordName { get; private set; }
        public string? RecordLocation { get; private set; }
        public DateTime RecordCreateTime { get; init; }
        public DateTime LikeTime { get; init; }
        public UserLike(
            string sharedRecordObjectId,
            string userId,string userName,string userAvatarUrl,
            string? recordTitle,string recordText,string? recordMusicRecordName,string? recordLocation,DateTime recordCreateTime,
            DateTime likeTime
        )
        {
            Id = sharedRecordObjectId;
            UserId = userId;
            UserName = userName;
            UserAvatarUrl = userAvatarUrl;
            RecordTitle = recordTitle;
            RecordText = recordText;
            RecordMusicRecordName = recordMusicRecordName;
            RecordLocation = recordLocation;
            LikeTime = likeTime;
        }
    }
}
