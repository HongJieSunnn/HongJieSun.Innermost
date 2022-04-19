namespace Innermost.Identity.API.Models
{
    /// <summary>
    /// User model in Innermost
    /// </summary>
    public class InnermostUser : IdentityUser
    {
        /// <summary>
        /// Real name
        /// </summary>
        public string? Name { get; set; }

        [Range(1, 130, ErrorMessage = "Age must between 1 and 130")]
        public uint Age { get; set; }

        [RegularExpression(@"^MALE|FEMALE$", ErrorMessage = "Error gender.Gender just only can be MALE or FEMALE")]
        [Required, Column(TypeName = "VARCHAR(8)")]
        public string Gender { get; set; }

        [Required]
        [StringLength(maximumLength: 18, MinimumLength = 1)]
        public string NickName { get; set; }

        [StringLength(maximumLength: 80)]
        public string? School { get; set; }

        [StringLength(30)]
        public string? Province { get; set; }

        [StringLength(30)]
        public string? City { get; set; }

        [StringLength(maximumLength: 150)]
        public string SelfDescription { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "Birthday length must be 10 and accords with Birthday pattern.")]
        [RegularExpression(@"(19|20)\d{2}-(1[0-2]|0[1-9])-(0[1-9]|[1-2][0-9]|3[0-1])", ErrorMessage = "Birthday pattern error.It must like yyyy-mm-dd")]//Pattern is yyyy-mm-dd (if month less than 10 it will be 0m)
        public string Birthday { get; set; }

        //TODO userUrls
        //public string UserAvatarUrl { get; set; }
        //public string UserBackgroundImageUrl { get; set; }


        [Required, Column(TypeName = "DATETIME")]
        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? DeleteTime { get; set; }
        public InnermostUser()
        {
            //TODO
        }
    }
}
