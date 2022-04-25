﻿using CommonIdentityService.IdentityService;
using Innermost.Identity.API.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddGrpcClient<IdentityUserGrpc.IdentityUserGrpcClient>();

            services
                .AddGrpcClient<IdentityUserGrpc.IdentityUserGrpcClient>(options =>
                {
                    options.Address = new Uri("https://localhost:5106");
                });

            //We call grpc to Identity.API in services' actions and which means that if we can call these grpcs,we have already been verified.

            return services;
        }
    }
}
