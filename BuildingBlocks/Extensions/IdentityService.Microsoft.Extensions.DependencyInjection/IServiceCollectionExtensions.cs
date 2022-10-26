using Innermost.IdentityService;
using Innermost.IdentityService.Abstractions;
using Innermost.IdentityService.Grpc;
using Microsoft.AspNetCore.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Add IHttpContextAccessor and IdentityService denpendencies.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IIdentityService, IdentityServiceBase>();

            return services;
        }

        /// <summary>
        /// Add UserIdentityService and dependencies it needs.(IHttpContextAccessor and IdentityUserGrpc.IdentityUserGrpcClient)
        /// </summary>
        public static IServiceCollection AddIdentityProfileService(this IServiceCollection services, string grpcAddress)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IIdentityService, IdentityProfileService>();
            services.AddScoped<IIdentityProfileService, IdentityProfileService>();
            services.AddGrpcClient<IdentityProfileGrpc.IdentityProfileGrpcClient>();

            services
                .AddGrpcClient<IdentityProfileGrpc.IdentityProfileGrpcClient>(options =>
                {
                    options.Address = new Uri(grpcAddress);
                });

            //We call grpc to Identity.API in services' actions and which means that if we can call these grpcs,we have already been verified.

            return services;
        }
    }
}
