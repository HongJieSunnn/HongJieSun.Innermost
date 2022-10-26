namespace Innermost.IdentityService.Abstractions
{
    public interface IIdentityProfileService : IIdentityService
    {
        Task<string> GetUserNameAsync(string? userId = null);
        Task<string> GetUserNickNameAsync(string? userId = null);
        Task<(string userName, string userNickName)> GetUserNamesAsync(string? userId = null);
        Task<string> GetUserAvatarUrlAsync(string? userId = null);
        Task<UserProfile> GetUserProfileAsync(string? userId = null);
        Task<UserProfileForLike> GetUserProfileForLikeAsync(string userId);
        Task<UserProfileSummary> GetUserProfileSummaryAsync(string? userId = null);
    }
}
