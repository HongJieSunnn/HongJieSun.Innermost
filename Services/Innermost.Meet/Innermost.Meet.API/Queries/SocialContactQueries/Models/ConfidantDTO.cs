namespace Innermost.Meet.API.Queries.SocialContactQueries.Models
{
    public class ConfidantDTO
    {
        public string ConfidantUserId { get; set; }
        public string ConfidantUserName { get; set; }
        public string ConfidantUserNickName { get; set; }
        public string ConfidantAvatarUrl { get; set; }
        public bool ConfidantOnline { get; set; }
        public string ConfidantStatue { get; set; }
        public string ChattingContextId { get; set; }

        public ConfidantDTO(string confidantUserId, string confidantUserName, string confidantUserNickName, string confidantAvatarUrl, string confidantStatue, bool confidantOnline, string chattingContextId)
        {
            ConfidantUserId = confidantUserId;
            ConfidantUserName = confidantUserName;
            ConfidantUserNickName = confidantUserNickName;
            ConfidantAvatarUrl = confidantAvatarUrl;
            ConfidantStatue = confidantStatue;
            ConfidantOnline = confidantOnline;
            ChattingContextId = chattingContextId;
        }
    }
}
