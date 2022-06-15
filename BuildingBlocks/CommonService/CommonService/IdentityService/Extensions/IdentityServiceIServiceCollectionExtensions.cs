using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CommonService.IdentityService.Extensions
{
    public static class IdentityServiceIServiceCollectionExtensions
    {
        /// <summary>
        /// Add IHttpContextAccessor and IdentityService denpendencies.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IIdentityService, IdentityService>();

            return services;
        }
    }
}
