namespace Innermost.IdentityService.Models
{
    public class UserProfileForLike
    {
        public string UserName { get; set; }
        public string UserNickName { get; set; }
        public string UserAvatarUrl { get; set; }
        public UserProfileForLike(string userName, string userNickName, string userAvatarUrl)
        {
            UserName = userName;
            UserNickName = userNickName;
            UserAvatarUrl = userAvatarUrl;
        }
    }
}
