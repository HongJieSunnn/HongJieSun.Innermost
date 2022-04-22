using CommonIdentityService.Models;
using Innermost.Identity.API;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CommonIdentityService.IdentityService
{
    /// <summary>
    /// Microservice use this service should inject IdentityUserGrpc.IdentityUserGrpcClient in ServiceCollection by service.AddGrpcClient.
    /// </summary>
    /// <typeparam name="TGrpcClient"></typeparam>
    public class UserIdentityService : IUserIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IdentityUserGrpc.IdentityUserGrpcClient _identityUserGrpcClient;
        public UserIdentityService(IHttpContextAccessor httpContextAccessor, IdentityUserGrpc.IdentityUserGrpcClient identityUserGrpcClient)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityUserGrpcClient = identityUserGrpcClient;
        }

        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }

        public async Task<string> GetUserAvatarUrlAsync(string? userId = null)
        {
            var userIdGrpcDTO = new UserIdGrpcDTO() { UserId = userId ?? GetUserId() };
            //TODO I don't know if the Async mthod can run correctly.Because we override not Async method in IdentityUserGrpcService which return type is Task.
            var userAvatarUrlGrpcDTO = await _identityUserGrpcClient.GetUserProfileForLikeAsync(userIdGrpcDTO);
            return userAvatarUrlGrpcDTO.UserAvatarUrl;
        }

        public async Task<string> GetUserNameAsync(string? userId = null)
        {
            var userIdGrpcDTO = new UserIdGrpcDTO() { UserId = userId ?? GetUserId() };
            var userNamesGrpcDTO = await _identityUserGrpcClient.GetUserProfileForLikeAsync(userIdGrpcDTO);
            return userNamesGrpcDTO.UserName;
        }

        public async Task<string> GetUserNickNameAsync(string? userId = null)
        {
            var userIdGrpcDTO = new UserIdGrpcDTO() { UserId = userId ?? GetUserId() };
            var userNamesGrpcDTO = await _identityUserGrpcClient.GetUserProfileForLikeAsync(userIdGrpcDTO);
            return userNamesGrpcDTO.UserNickName;
        }

        public async Task<(string userName, string userNickName)> GetUserNamesAsync(string? userId = null)
        {
            var userIdGrpcDTO = new UserIdGrpcDTO() { UserId = userId ?? GetUserId() };
            var userNamesGrpcDTO = await _identityUserGrpcClient.GetUserProfileForLikeAsync(userIdGrpcDTO);
            return (userNamesGrpcDTO.UserName, userNamesGrpcDTO.UserNickName);
        }

        public async Task<UserProfile> GetUserProfileAsync(string? userId = null)
        {
            var userIdGrpcDTO = new UserIdGrpcDTO() { UserId = userId ?? GetUserId() };
            var userProfileGrpcDTO = await _identityUserGrpcClient.GetUserProfileAsync(userIdGrpcDTO);
            return new UserProfile(
                userProfileGrpcDTO.UserName, userProfileGrpcDTO.UserNickName, userProfileGrpcDTO.UserEmail,userProfileGrpcDTO.UserStatue,
                userProfileGrpcDTO.RealName, userProfileGrpcDTO.Age, userProfileGrpcDTO.Gender, userProfileGrpcDTO.School,
                userProfileGrpcDTO.Province, userProfileGrpcDTO.City,
                userProfileGrpcDTO.SelfDescription, userProfileGrpcDTO.Birthday,userProfileGrpcDTO.UserAvatarUrl,userProfileGrpcDTO.UserBackgroundImageUrl, userProfileGrpcDTO.CreateTime);
        }

        public async Task<UserProfileForLike> GetUserProfileForLikeAsync(string userId)
        {
            var userIdGrpcDTO = new UserIdGrpcDTO() { UserId = userId};
            var userProfileForLikeGrpcDTO = await _identityUserGrpcClient.GetUserProfileForLikeAsync(userIdGrpcDTO);

            return new UserProfileForLike(userProfileForLikeGrpcDTO.UserName, userProfileForLikeGrpcDTO.UserNickName, userProfileForLikeGrpcDTO.UserAvatarUrl);
        }

        public async Task<UserProfileSummary> GetUserProfileSummaryAsync(string? userId = null)
        {
            var userIdGrpcDTO = new UserIdGrpcDTO() { UserId = userId };
            var userProfileSummaryGrpcDTO = await _identityUserGrpcClient.GetUserProfileSummaryAsync(userIdGrpcDTO);

            return new UserProfileSummary(
                userProfileSummaryGrpcDTO.UserName, userProfileSummaryGrpcDTO.UserNickName, 
                userProfileSummaryGrpcDTO.SelfDescription, 
                userProfileSummaryGrpcDTO.UserAvatarUrl, userProfileSummaryGrpcDTO.UserBackgroundImageUrl);
        }
    }
}
