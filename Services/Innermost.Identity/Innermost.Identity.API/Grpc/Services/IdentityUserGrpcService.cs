using Grpc.Core;
using Innermost.Identity.API.User;

namespace Innermost.Identity.API.Grpc.Services
{
    public class IdentityUserGrpcService:IdentityUserGrpc.IdentityUserGrpcBase
    {
        private readonly UserManager<InnermostUser> _userManager;
        public IdentityUserGrpcService(UserManager<InnermostUser> userManager)
        {
            _userManager = userManager;
        }
        public override async Task<UserGrpcDTO> GetUserProfile(UserIdGrpcDTO request, ServerCallContext context)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            return new UserGrpcDTO()
            {
                UserName = user.UserName,
                UserNickName = user.NickName,
                UserEmail = user.Email,
                UserStatue=user.UserStatue,
                RealName = user.RealName,
                Age = user.Age,
                Gender = user.Gender,
                School = user.School,
                Province = user.Province,
                City = user.City,
                SelfDescription = user.SelfDescription,
                Birthday = user.Birthday,
                UserAvatarUrl=user.UserAvatarUrl,
                UserBackgroundImageUrl=user.UserBackgroundImageUrl,
                CreateTime = user.CreateTime.ToString()
            };
        }
        public override async Task<UserProfileForLikeGrpcDTO> GetUserProfileForLike(UserIdGrpcDTO request, ServerCallContext context)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            return new UserProfileForLikeGrpcDTO()
            {
                UserName = user.UserName,
                UserNickName = user.NickName,
                UserAvatarUrl = user.UserAvatarUrl,
            };
        }

        public override async Task<UserProfileSummaryGrpcDTO> GetUserProfileSummary(UserIdGrpcDTO request, ServerCallContext context)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            return new UserProfileSummaryGrpcDTO()
            {
                UserName = user.UserName,
                UserNickName = user.NickName,
                SelfDescription = user.SelfDescription,
                UserAvatarUrl = user.UserAvatarUrl,
                UserBackgroundImageUrl = user.UserBackgroundImageUrl,
            };
        }
    }
}
