namespace Innermost.Identity.API.Models
{
    public record RegisterModel
    {
        [RegularExpression(@"[a-zA-Z0-9_-]{4,20}", ErrorMessage = "UserName only can contain letters,numbers,- and _")]
        public string UserName { get; init; }

        [EmailAddress]
        public string Email { get; init; }

        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long", MinimumLength = 8)]
        public string Password { get; init; }

        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; init; }

        [RegularExpression(@"[a-zA-Z0-9_-]{1,30}", ErrorMessage = "NickName only can contain letters,numbers,- and _")]
        public string NickName { get; set; }
        public DateTime Birthday { get; set; }
        public string UserAvatarUrl { get; set; }

        [Url]
        public string UserBackgroundImageUrl { get; set; }

        [RegularExpression(@"^MALE|FEMALE|OTHER$", ErrorMessage = "Error gender.Gender just only can be MALE,FEMALE and OTHER.")]
        public string Gender { get; set; }

        [StringLength(maximumLength: 150)]
        public string? SelfDescription { get; set; }

        [StringLength(maximumLength: 80)]
        public string? School { get; set; }

        [StringLength(30)]
        public string? Province { get; set; }

        [StringLength(30)]
        public string? City { get; set; }

        public RegisterModel(
            string userName,
            string email,
            string password,
            string confirmPassword,
            string nickName,
            DateTime birthday,
            string userAvatarUrl,
            string userBackgroundImageUrl,
            string gender,
            string? selfDescription,
            string? school,
            string? province,
            string? city
        )
        {
            UserName = userName;
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
            NickName = nickName;
            Birthday = birthday;
            UserAvatarUrl = userAvatarUrl;
            UserBackgroundImageUrl = userBackgroundImageUrl;
            Gender = gender;
            SelfDescription = selfDescription;
            School = school;
            Province = province;
            City = city;
        }
    }
}
