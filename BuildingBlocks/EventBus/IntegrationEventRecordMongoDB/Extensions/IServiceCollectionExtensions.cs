using Innermost.MongoDBContext;
using IntegrationEventRecordMongoDB.Services;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using System.Reflection;

namespace IntegrationEventRecordMongoDB.Extensions.Microsoft.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationEventRecordMongoDB(this IServiceCollection services)
        {
            AddIntegrationEventDiscriminators();

            services.AddSingleton<IIntegrationEventRecordMongoDBService, IntegrationEventRecordMongoDBService>(s =>
            {
                var context=s.GetRequiredService<MongoDBContextBase>();
                var database=context.Database;
                return new IntegrationEventRecordMongoDBService(database);
            });

            return services;
        }

        /// <summary>
        /// To register IntegrationEvent discriminators.
        /// </summary>
        /// <param name="integrationEventTypes"></param>
        public static void AddIntegrationEventDiscriminators()
        {
            var integrationEventTypes = Assembly.Load(Assembly.GetEntryAssembly().FullName)
                .GetTypes()
                .Where(t => t.BaseType == typeof(IntegrationEvent) || t.Name.EndsWith("IntegrationEvent"))//So we should named integrationEvent
                .ToList();

            foreach (var type in integrationEventTypes)
            {
                BsonClassMap.RegisterClassMap(new BsonClassMap(type));
                //TODO if this line causes bug and we should use RegisterClassMap<TClass> or we need move this to configure app method.
            }
        }
    }
}
