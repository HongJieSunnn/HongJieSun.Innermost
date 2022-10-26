namespace Innermost.IdentityService.Abstractions
{
    public interface IIdentityService
    {
        /// <summary>
        /// Get logged in user id.
        /// </summary>
        /// <returns>logged in user id</returns>
        string GetUserId();
    }
}
