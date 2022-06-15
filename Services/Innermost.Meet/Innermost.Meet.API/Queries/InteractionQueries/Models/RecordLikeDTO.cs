namespace Innermost.Meet.API.Queries.InteractionQueries.Models
{
    public class RecordLikeDTO
    {
        public string SharedRecordObjectId { get; init; }
        public string RecordUserId { get; init; }
        public string RecordUserName { get; init; }
        public string RecordUserNickName { get; init; }
        public string RecordUserAvatarUrl { get; init; }
        public string? RecordTitle { get; init; }
        public string RecordText { get; init; }
        public string? RecordMusicRecordName { get; init; }
        public string? RecordLocation { get; init; }
        public DateTime RecordCreateTime { get; init; }
        public DateTime LikeTime { get; init; }
        public RecordLikeDTO(
            string sharedRecordObjectId,
            string recordUserId, string recordUserName, string recordUserNickName, string recordUserAvatarUrl,
            string? recordTitle, string recordText, string? recordMusicRecordName, string? recordLocation,
            DateTime recordCreateTime, DateTime likeTime)
        {
            SharedRecordObjectId = sharedRecordObjectId;
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
