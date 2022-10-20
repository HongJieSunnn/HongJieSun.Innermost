using EventBusCommon.Abstractions;
using IntegrationEventServiceSQL.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IntegrationEventServiceSQL.Extensions
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Use this method in the service which need send integration events and use sql db.
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIntegrationEventServiceSQL<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            services.AddSingleton<IIntegrationEventService, CommonIntegrationEventServiceSQL<TDbContext>>(s =>
            {
                var context = s.GetRequiredService<TDbContext>();
                var eventBus = s.GetRequiredService<IAsyncEventBus>();
                var logger = s.GetRequiredService<ILogger<CommonIntegrationEventServiceSQL<TDbContext>>>();

                return new CommonIntegrationEventServiceSQL<TDbContext>(context, eventBus, logger);
            });

            return services;
        }
    }
}
