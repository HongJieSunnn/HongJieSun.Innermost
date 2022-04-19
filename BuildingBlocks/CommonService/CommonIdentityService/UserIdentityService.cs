using CommonIdentityService.IdentityService.Models;
using Innermost.Identity.API;
using Microsoft.AspNetCore.Http;

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
        public UserIdentityService(IHttpContextAccessor httpContextAccessor,IdentityUserGrpc.IdentityUserGrpcClient identityUserGrpcClient)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityUserGrpcClient=identityUserGrpcClient;
        }

        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst("sub")!.Value;
        }

        public async Task<string> GetUserAvatarUrlAsync(string? userId = null)
        {
            var userIdDTO = new UserIdDTO() { UserId = userId?? GetUserId() };
            //TODO I don't know if the Async mthod can run correctly.Because we override not Async method in IdentityUserGrpcService which return type is Task.
            var userAvatarUrlDTO = await _identityUserGrpcClient.GetUserAvatarUrlAsync(userIdDTO);
            return userAvatarUrlDTO.UserAvatarUrl;
        }

        public async Task<string> GetUserNameAsync(string? userId = null)
        {
            var userIdDTO = new UserIdDTO() { UserId = userId ?? GetUserId() };
            var userNamesDTO = await _identityUserGrpcClient.GetUserNamesAsync(userIdDTO);
            return userNamesDTO.UserName;
        }

        public async Task<string> GetUserNickNameAsync(string? userId = null)
        {
            var userIdDTO = new UserIdDTO() { UserId = userId ?? GetUserId() };
            var userNamesDTO = await _identityUserGrpcClient.GetUserNamesAsync(userIdDTO);
            return userNamesDTO.UserNickName;
        }

        public async Task<(string userName, string userNickName)> GetUserNamesAsync(string? userId = null)
        {
            var userIdDTO = new UserIdDTO() { UserId = userId ?? GetUserId() };
            var userNamesDTO = await _identityUserGrpcClient.GetUserNamesAsync(userIdDTO);
            return (userNamesDTO.UserName, userNamesDTO.UserNickName);
        }

        public async Task<UserProfile> GetUserProfileAsync(string? userId = null)
        {
            var userIdDTO = new UserIdDTO() { UserId = userId ?? GetUserId() };
            var userProfileDTO = await _identityUserGrpcClient.GetUserProfilesAsync(userIdDTO);
            return new UserProfile(
                userProfileDTO.UserName, userProfileDTO.UserNickName, userProfileDTO.UserEmail, 
                userProfileDTO.Name, userProfileDTO.Age, userProfileDTO.Gender, userProfileDTO.School, 
                userProfileDTO.Province, userProfileDTO.City, 
                userProfileDTO.SelfDescription, userProfileDTO.Birthday, userProfileDTO.CreateTime);
        }
    }
}
