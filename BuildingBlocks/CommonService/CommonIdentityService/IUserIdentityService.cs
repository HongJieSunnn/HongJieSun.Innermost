using CommonIdentityService.IdentityService.Models;

namespace CommonIdentityService.IdentityService
{
    /// <summary>
    /// To avoid too many dependencies while service use CommonService,I take UserIdentityService to a new project.
    /// So that the service which just need GetUserId function will not depend on the Grpc Client dependencies.
    /// </summary>
    public interface IUserIdentityService
    {
        /// <summary>
        /// Get logged in user id.
        /// </summary>
        /// <returns>logged in user id</returns>
        string GetUserId();
        Task<string> GetUserNameAsync(string? userId=null);
        Task<string> GetUserNickNameAsync(string? userId = null);
        Task<(string userName, string userNickName)> GetUserNamesAsync(string? userId = null);
        Task<string> GetUserAvatarUrlAsync(string? userId = null);
        Task<UserProfile> GetUserProfileAsync(string? userId = null);
    }
}
