namespace Innermost.IdentityService.Models
{
    public class UserProfileSummary
    {
        public string UserName { get; set; }
        public string UserNickName { get; set; }
        public string SelfDescription { get; set; }
        public string UserAvatarUrl { get; set; }
        public string UserBackgroundImageUrl { get; set; }
        public UserProfileSummary(string userName, string userNickName, string selfDescription, string userAvatarUrl, string userBackgroundImageUrl)
        {
            UserName = userName;
            UserNickName = userNickName;
            SelfDescription = selfDescription;
            UserAvatarUrl = userAvatarUrl;
            UserBackgroundImageUrl = userBackgroundImageUrl;
        }
    }
}
