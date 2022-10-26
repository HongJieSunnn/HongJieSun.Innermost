namespace Innermost.IdentityService.Models
{
    public class UserProfile
    {
        public string UserName { get; set; }
        public string UserNickName { get; set; }
        public string UserEmail { get; set; }
        public string UserStatue { get; set; }
        public string RealName { get; set; }
        public uint Age { get; set; }
        public string Gender { get; set; }
        public string School { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string SelfDescription { get; set; }
        public string Birthday { get; set; }
        public string UserAvatarUrl { get; set; }
        public string UserBackgroundImageUrl { get; set; }
        public string CreateTime { get; set; }
        public UserProfile(
            string userName, string userNickName, string userEmail, string userStatue,
            string realName, uint age, string gender,
            string school, string province, string city,
            string selfDescription, string birthday, string userAvatarUrl, string userbackgroundImageUrl, string createTime)
        {
            UserName = userName;
            UserNickName = userNickName;
            UserEmail = userEmail;
            UserStatue = userStatue;
            RealName = realName;
            Age = age;
            Gender = gender;
            School = school;
            Province = province;
            City = city;
            SelfDescription = selfDescription;
            Birthday = birthday;
            UserAvatarUrl = userAvatarUrl;
            UserBackgroundImageUrl = userbackgroundImageUrl;
            CreateTime = createTime;
        }
    }
}
