using Grpc.Core;
using StackExchange.Redis;

namespace Innermost.Identity.API.Grpc.Services
{
    public class IdentityUserStatueGrpcService:IdentityUserStatueGrpc.IdentityUserStatueGrpcBase
    {
        private readonly UserStatueRedisContext _redisContext;
        private const string UserOnlineStatueKey = "user_online_statue";
        private const string UserStatueKey = "user_statue";
        public IdentityUserStatueGrpcService(UserStatueRedisContext redisContext)
        {
            _redisContext = redisContext;

        }
        public override async Task<IsUserOnlineGrpcDTO> IsUserOnline(IsUserOnlineUserIdGrpcDTO request, ServerCallContext context)
        {
            var onlineStatueRedisValues=await _redisContext.Context().HashGetAsync(UserOnlineStatueKey,request.UserId);
            return new IsUserOnlineGrpcDTO() { IsOnline = (bool)onlineStatueRedisValues };
        }
        public override async Task<UsersOnlineStatueGrpcDTO> GetUsersOnlineStatue(UserIdsGrpcDTO request, ServerCallContext context)
        {
            var onlineStatueRedisValues = await _redisContext.Context().HashGetAsync(UserOnlineStatueKey, request.UserIds.Select(id=>new RedisValue(id)).ToArray());
            var usersOnlineStatueDTO = new UsersOnlineStatueGrpcDTO();//repeated field properties is get only.So we should add values by add.

            usersOnlineStatueDTO.UsersOnlineStatue.AddRange(onlineStatueRedisValues.Select(rv=>(bool)rv));

            return usersOnlineStatueDTO;
        }

        public override async Task<UsersStatueGrpcDTO> GetUsersStatue(UserIdsGrpcDTO request, ServerCallContext context)
        {
            var onlineStatueRedisValues = await _redisContext.Context().HashGetAsync(UserStatueKey, request.UserIds.Select(id => new RedisValue(id)).ToArray());
            var usersStatueDTO = new UsersStatueGrpcDTO();

            usersStatueDTO.UsersStatue.AddRange(onlineStatueRedisValues.Select(rv => (string)rv));

            return usersStatueDTO;
        }

        public override async Task<SetUserStatueVoidRetGrpcDTO> SetUserOnlineStatue(SetUserOnlineStatueGrpcDTO request, ServerCallContext context)
        {
            await _redisContext.Context().HashSetAsync(UserOnlineStatueKey, request.UserId, request.IsOnline);
            return new SetUserStatueVoidRetGrpcDTO();
        }

        public override async Task<SetUserStatueVoidRetGrpcDTO> SetUserStatue(SetUserStatueGrpcDTO request, ServerCallContext context)
        {
            await _redisContext.Context().HashSetAsync(UserStatueKey, request.UserId, request.UserStatue);
            return new SetUserStatueVoidRetGrpcDTO();
        }
    }
}
