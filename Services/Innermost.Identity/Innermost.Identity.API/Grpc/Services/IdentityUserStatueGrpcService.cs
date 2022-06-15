using Grpc.Core;
using Innermost.Identity.API.UserStatue;

namespace Innermost.Identity.API.Grpc.Services
{
    public class IdentityUserStatueGrpcService : IdentityUserStatueGrpc.IdentityUserStatueGrpcBase
    {
        private readonly IUserStatueService _userStatueService;
        public IdentityUserStatueGrpcService(IUserStatueService userStatueService)
        {
            _userStatueService = userStatueService;
        }
        public override async Task<IsUserOnlineGrpcDTO> IsUserOnline(IsUserOnlineUserIdGrpcDTO request, ServerCallContext context)
        {
            var onlineStatueRedisValue = await _userStatueService.IsUserOnlinedAsync(request.UserId);
            return new IsUserOnlineGrpcDTO() { IsOnline = onlineStatueRedisValue };
        }
        public override async Task<UsersOnlineStatueGrpcDTO> GetUsersOnlineStatue(UserIdsGrpcDTO request, ServerCallContext context)
        {
            var onlineStatueRedisValues = await _userStatueService.GetUsersOnlinedStatueAsync(request.UserIds);
            var usersOnlineStatueDTO = new UsersOnlineStatueGrpcDTO();//repeated field properties is get only.So we should add values by add.

            usersOnlineStatueDTO.UsersOnlineStatues.AddRange(onlineStatueRedisValues);

            return usersOnlineStatueDTO;
        }

        public override async Task<UsersStatueGrpcDTO> GetUsersStatue(UserIdsGrpcDTO request, ServerCallContext context)
        {
            var onlineStatueRedisValues = await _userStatueService.GetUsersStatueAsync(request.UserIds);
            var usersStatueDTO = new UsersStatueGrpcDTO();

            usersStatueDTO.UsersStatues.AddRange(onlineStatueRedisValues);

            return usersStatueDTO;
        }

        public override async Task<SetUserStatueVoidRetGrpcDTO> SetUserOnlineStatue(SetUserOnlineStatueGrpcDTO request, ServerCallContext context)
        {
            await _userStatueService.SetUserOnlineStatueAsync(request.UserId, request.IsOnline);
            return new SetUserStatueVoidRetGrpcDTO();
        }

        public override async Task<SetUserStatueVoidRetGrpcDTO> SetUserStatue(SetUserStatueGrpcDTO request, ServerCallContext context)
        {
            await _userStatueService.SetUserStatueAsync(request.UserId, request.UserStatue);
            return new SetUserStatueVoidRetGrpcDTO();
        }
    }
}
