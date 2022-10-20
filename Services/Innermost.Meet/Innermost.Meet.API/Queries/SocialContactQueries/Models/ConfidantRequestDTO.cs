namespace Innermost.Meet.API.Queries.SocialContactQueries.Models
{
    public class ConfidantRequestDTO
    {
        public string RequestId { get; init; }
        public string RequestUserId { get; init; }
        public string RequestUserName { get; init; }
        public string RequestUserNickName { get; init; }
        public string RequestUserAvatarUrl { get; init; }
        public string RequestMessage { get; init; }
        public string ConfidantRequestStatue { get; init; }
        public DateTime RequestTime { get; init; }
        public DateTime? UpdateTime { get; init; }
        public ConfidantRequestDTO(string requestId, string requestUserId, string requestUserName, string requestUserNickName, string requestUserAvatarUrl, string requestMessage, string confidantRequestStatue, DateTime requestTime, DateTime? updateTime)
        {
            RequestId = requestId;
            RequestUserId = requestUserId;
            RequestUserName = requestUserName;
            RequestUserNickName = requestUserNickName;
            RequestUserAvatarUrl = requestUserAvatarUrl;
            RequestMessage = requestMessage;
            ConfidantRequestStatue = confidantRequestStatue;
            RequestTime = requestTime;
            UpdateTime = updateTime;
        }
    }
}
