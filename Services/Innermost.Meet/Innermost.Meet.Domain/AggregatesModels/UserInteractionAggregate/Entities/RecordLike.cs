namespace Innermost.Meet.Domain.AggregatesModels.UserInteractionAggregate.Entities
{
    /// <summary>
    /// User like contains the liked record's summary.
    /// Id is sharedRecordObjectId.
    /// </summary>
    public class RecordLike : Entity<string>
    {
        public string RecordUserId { get; init; }
        public string RecordUserName { get; private set; }
        public string RecordUserNickName { get; private set; }
        public string RecordUserAvatarUrl { get; private set; }
        public string? RecordTitle { get; private set; }
        public string RecordText { get; private set; }
        public string? RecordMusicRecordName { get; private set; }
        public string? RecordLocation { get; private set; }
        public DateTime RecordCreateTime { get; init; }
        public DateTime LikeTime { get; init; }
        public RecordLike(
            string sharedRecordObjectId,
            string recordUserId, string recordUserName, string recordUserNickName, string recordUserAvatarUrl,
            string? recordTitle, string recordText, string? recordMusicRecordName, string? recordLocation, 
            DateTime recordCreateTime,DateTime likeTime)
        {
            Id = sharedRecordObjectId;
            RecordUserId = recordUserId;
            RecordUserName = recordUserName;
            RecordUserNickName = recordUserNickName;
            RecordUserAvatarUrl = recordUserAvatarUrl;
            RecordTitle = recordTitle;
            RecordText = recordText;
            RecordMusicRecordName = recordMusicRecordName;
            RecordLocation = recordLocation;
            RecordCreateTime = recordCreateTime;
            LikeTime = likeTime;
        }
    }
}
