namespace Innermost.Identity.API.Services.UserStatueServices
{
    public interface IUserStatueService
    {
        Task<bool> IsUserOnlinedAsync(string userId);
        Task<IEnumerable<bool>> GetUsersOnlinedStatueAsync(IEnumerable<string> userIds);
        Task<IEnumerable<string>> GetUsersStatueAsync(IEnumerable<string> userIds);
        Task SetUserOnlineStatueAsync(string userId, bool online);
        Task SetUserStatueAsync(string userId, string statue);
        Task SetUserStatueFromMySQL(InnermostUser user);
    }
}
