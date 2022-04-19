using Grpc.Core;

namespace Innermost.Identity.API.Grpc.Services
{
    public class IdentityUserGrpcService:IdentityUserGrpc.IdentityUserGrpcBase
    {
        private readonly UserManager<InnermostUser> _userManager;
        public IdentityUserGrpcService(UserManager<InnermostUser> userManager)
        {
            _userManager=userManager;
        }
        public override async Task<UserAvatarUrlDTO> GetUserAvatarUrl(UserIdDTO request, ServerCallContext context)
        {
            var user =await _userManager.FindByIdAsync(request.UserId);
            throw new NotImplementedException();
        }
        public override async Task<UserNamesDTO> GetUserNames(UserIdDTO request, ServerCallContext context)
        {
            var user=await _userManager.FindByNameAsync(request.UserId);
            return new UserNamesDTO() { UserName = user.UserName , UserNickName = user.NickName};
        }
        public override async Task<UserDTO> GetUserProfiles(UserIdDTO request, ServerCallContext context)
        {
            var user = await _userManager.FindByNameAsync(request.UserId);
            return new UserDTO()
            {
                UserName = user.UserName,
                UserNickName = user.NickName,
                UserEmail = user.Email,
                Name = user.Name,
                Age = user.Age,
                Gender = user.Gender,
                School = user.School,
                Province = user.Province,
                City = user.City,
                SelfDescription = user.SelfDescription,
                Birthday = user.Birthday,
                //todo UserAvatarUrl=
                //todo UserBackgroundImageUrl=
                CreateTime = user.CreateTime.ToString()
            };
        }
    }
}
