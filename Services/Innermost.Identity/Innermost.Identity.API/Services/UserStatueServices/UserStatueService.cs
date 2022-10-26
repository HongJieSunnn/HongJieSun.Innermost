using StackExchange.Redis;

namespace Innermost.Identity.API.Services.UserStatueServices
{
    public class UserStatueService : IUserStatueService
    {
        private readonly UserStatueRedisContext _redisContext;
        private readonly UserManager<InnermostUser> _userManager;
        private const string UserOnlineStatueKey = "user_online_statue";
        private const string UserStatueKey = "user_statue";
        public UserStatueService(UserStatueRedisContext redisContext, UserManager<InnermostUser> userManager)
        {
            _redisContext = redisContext;
            _userManager = userManager;
        }

        public async Task<bool> IsUserOnlinedAsync(string userId)
        {
            var onlineStatueRedisValue = await _redisContext.Context().HashGetAsync(UserOnlineStatueKey, userId);

            return (bool)onlineStatueRedisValue;
        }

        public async Task<IEnumerable<bool>> GetUsersOnlinedStatueAsync(IEnumerable<string> userIds)
        {
            var onlineStatueRedisValues = await _redisContext.Context().HashGetAsync(UserOnlineStatueKey, userIds.Select(id => new RedisValue(id)).ToArray());

            return onlineStatueRedisValues.Select(rv => (bool)rv);
        }

        public async Task<IEnumerable<string>> GetUsersStatueAsync(IEnumerable<string> userIds)
        {
            var onlineStatueRedisValues = await _redisContext.Context().HashGetAsync(UserStatueKey, userIds.Select(id => new RedisValue(id)).ToArray());

            return onlineStatueRedisValues.Select(rv => rv.ToString());
        }

        public async Task SetUserOnlineStatueAsync(string userId, bool online)
        {
            await _redisContext.Context().HashSetAsync(UserOnlineStatueKey, userId, online);
        }

        public async Task SetUserStatueAsync(string userId, string statue)
        {
            await SetUserStatueToMySQLAsync(userId, statue);
            await _redisContext.Context().HashSetAsync(UserStatueKey, userId, statue);
        }

        private async Task SetUserStatueToMySQLAsync(string userId, string statue)
        {
            var userTask = _userManager.FindByIdAsync(userId);

            var user = await userTask;
            user.UserStatue = statue;

            await _userManager.UpdateAsync(user);

            await SetUserStatueToClaimAsync(user, statue);
        }

        private async Task SetUserStatueToClaimAsync(InnermostUser user, string statue)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            var userStatueClaim = userClaims.FirstOrDefault(uc => uc.Type == "user_statue");

            if (userStatueClaim is not null)
                await _userManager.ReplaceClaimAsync(user, userStatueClaim, new Claim("user_statue", statue));
        }

        public async Task SetUserStatueFromMySQL(InnermostUser user)
        {
            var userStatueInRedis = await _redisContext.Context().HashGetAsync(UserStatueKey, user.Id);

            if (userStatueInRedis == RedisValue.Null || userStatueInRedis != user.UserStatue)
            {
                await _redisContext.Context().HashSetAsync(UserStatueKey, user.Id, user.UserStatue);
            }
        }
    }
}
