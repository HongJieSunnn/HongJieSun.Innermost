using CommonIdentityService.IdentityService;
using Grpc.Core;
using Innermost.Identity.API;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace CommonIdentityService.Extensions
{
    public static class UserIdentityServiceIServiceCollectionExtensions
    {
        /// <summary>
        /// Add UserIdentityService and dependencies it needs.(IHttpContextAccessor and IdentityUserGrpc.IdentityUserGrpcClient)
        /// </summary>
        public static IServiceCollection AddUserIdentityService(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserIdentityService, UserIdentityService>();

            services
                .AddGrpcClient<IdentityUserGrpc.IdentityUserGrpcClient>(options =>
                {
                    options.Address = new Uri("https://localhost:5106");
                });

            //We call grpc to Identity.API in services' actions and which means that if we can call these grpcs,we have already been verified.


                 //configure authentication in grpc
                //.ConfigureChannel((service, options) =>
                //{
                //    var httpContextAccessor = service.GetRequiredService<IHttpContextAccessor>();
                //    var token= httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization];

                //    //https://docs.microsoft.com/zh-cn/aspnet/core/grpc/authn-and-authz?view=aspnetcore-6.0
                //    //TODO I don't know if it's useful.
                //    var credentials = CallCredentials.FromInterceptor((context, metadata) =>
                //    {
                //        if (!string.IsNullOrEmpty(token))
                //        {
                //            metadata.Add("Authorization", $"Bearer {token}");
                //        }
                //        return Task.CompletedTask;
                //    });

                //    options.Credentials = ChannelCredentials.Create(new SslCredentials(), credentials);
                //});

            return services;
        }
    }
}
