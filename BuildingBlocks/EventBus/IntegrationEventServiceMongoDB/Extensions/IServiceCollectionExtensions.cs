namespace IntegrationEventServiceMongoDB.Extensions
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Use this method in the service which need send integration events and use mongodb.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIntegrationEventServiceMongoDB(this IServiceCollection services)
        {
            AddIntegrationEventDiscriminators();

            services.AddScoped<IIntegrationEventService, CommonIntegrationEventServiceMongoDB>(s =>
            {
                var context = s.GetRequiredService<MongoDBContextBase>();
                var database = context.Database;
                var eventBus = s.GetRequiredService<IAsyncEventBus>();
                var logger = s.GetRequiredService<ILogger<CommonIntegrationEventServiceMongoDB>>();

                var session = s.GetService<IClientSessionHandle>();

                return new CommonIntegrationEventServiceMongoDB(database, eventBus, logger, session);
            });

            return services;
        }

        /// <summary>
        /// To register IntegrationEvent discriminators.
        /// </summary>
        /// <param name="integrationEventTypes"></param>
        public static void AddIntegrationEventDiscriminators()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly is null)
                throw new InvalidOperationException($"EntryAssembly is null.");

            var tagClientAssembly = entryAssembly.GetReferencedAssemblies().FirstOrDefault(a => a.Name == "TagS.Microservices.Client");

            var integrationEventTypes = Assembly.Load(entryAssembly.FullName ?? throw new InvalidOperationException($"{entryAssembly}'s fullname is null."))
                .GetTypes()
                .Where(t => t.BaseType == typeof(IntegrationEvent))
                .ToList();

            if (tagClientAssembly is not null)
            {
                integrationEventTypes.AddRange(
                    Assembly.Load(tagClientAssembly.FullName)
                    .GetTypes()
                    .Where(t => t.BaseType == typeof(IntegrationEvent))
                );
            }

            foreach (var type in integrationEventTypes)
            {
                BsonClassMap.RegisterClassMap(new BsonClassMap(type));
                //TODO if this line causes bug and we should use RegisterClassMap<TClass> or we need move this to configure app method.
            }
        }
    }
}
