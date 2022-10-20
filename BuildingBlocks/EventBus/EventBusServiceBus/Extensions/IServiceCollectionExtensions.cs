using EventBusCommon;
using EventBusCommon.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventBusServiceBus.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDefaultAzureServiceBusEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var subcriptionName = configuration["SubscriptionClientName"] ?? throw new NullReferenceException("SubscriptionClientName is not contained in appsettings.json.");

            services.AddSingleton<IServiceBusPersisterConnection>(sp =>
            {
                var connectionString = configuration.GetSection("EventBusConnections")["ConnectAzureServiceBus"] ?? throw new NullReferenceException("EventBusConnections.ConnectAzureServiceBus is not contained in appsettings.json.");
                var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersisterConnection>>();

                return new DefaultServiceBusPersisterConnection(connectionString, logger);
            });

            services.AddSingleton<IAsyncEventBus, EventBusAzureServiceBus>(sp =>
            {
                var persister = sp.GetRequiredService<IServiceBusPersisterConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusAzureServiceBus>>();
                var subcriptionManager = sp.GetRequiredService<IEventBusSubscriptionManager>();
                var lifescope = sp.GetRequiredService<ILifetimeScope>();

                return new EventBusAzureServiceBus(persister, logger, subcriptionManager, subcriptionName, lifescope, subcriptionName);
            });

            services.AddSingleton<IEventBusSubscriptionManager, InMemoryEventBusSubscriptionsManager>();

            return services;
        }
    }
}
