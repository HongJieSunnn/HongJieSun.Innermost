using Microsoft.AspNetCore.Http;

namespace CommonService.IdentityService
{
    /// <summary>
    /// If only should get user id (like TagServer).Use this.
    /// </summary>
    public class IdentityService
        : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public string GetUserId()
        {
            //若要使用 FindFirst("sub") 需要先在 Startup 中 JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub"); 否则，需要 User.FindFirstValue(ClaimTypes.NameIdentifier);来获取
            //But TagServer has not configured that and IdentityService is also useful.
            return _httpContextAccessor.HttpContext.User.FindFirst("sub")!.Value;
        }
    }
}
