namespace Innermost.Identity.API.Models
{
    /// <summary>
    /// User model in Innermost
    /// </summary>
    public class InnermostUser : IdentityUser
    {

        [Range(1, 130, ErrorMessage = "Age must between 1 and 130")]
        public uint Age { get; set; }

        [RegularExpression(@"^MALE|FEMALE|OTHER$", ErrorMessage = "Error gender.Gender just only can be MALE,FEMALE and OTHER.")]
        [Required, Column(TypeName = "VARCHAR(8)")]
        public string Gender { get; set; }

        [StringLength(maximumLength: 30, MinimumLength = 1)]
        public string NickName { get; set; }

        [StringLength(maximumLength: 80)]
        public string? School { get; set; }

        [StringLength(30)]
        public string? Province { get; set; }

        [StringLength(30)]
        public string? City { get; set; }
        [StringLength(maximumLength: 150)]
        public string? SelfDescription { get; set; }
        public string? Birthday { get; set; }
        public string UserAvatarUrl { get; set; }
        public string UserBackgroundImageUrl { get; set; }

        [StringLength(maximumLength: 15, MinimumLength = 1)]
        [RegularExpression(@"^NORMAL|HAPPY|SAD|ANGRY|DEPRESSION|BORING|LAUGH|BAD|SPEECHLESS|FEAR|LONELY|RELEXED$")]
        public string UserStatue { get; set; }

        /// <summary>
        /// Real name
        /// </summary>
        public string? RealName { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? DeleteTime { get; set; }
        public InnermostUser()
        {

        }
        public InnermostUser(
            string userName,
            string email,
            int age,
            string gender,
            string nickName,
            string? school,
            string? province,
            string? city,
            string? selfDescription,
            string? birthday,
            string userAvatarUrl,
            string userBackgroundImageUrl,
            DateTime createTime,
            string userStatue = "NORMAL",
            string? realName = null,
            DateTime? updateTime = null,
            DateTime? deleteTime = null
        )
        {
            UserName = userName;
            Email = email;
            Age = (uint)age;
            Gender = gender;
            NickName = nickName;
            School = school;
            Province = province;
            City = city;
            SelfDescription = selfDescription;
            Birthday = birthday;
            UserAvatarUrl = userAvatarUrl;
            UserBackgroundImageUrl = userBackgroundImageUrl;
            CreateTime = createTime == default(DateTime) ? DateTime.Now : createTime;
            UserStatue = userStatue;
            RealName = realName;
            UpdateTime = updateTime;
            DeleteTime = deleteTime;
        }
    }
}
