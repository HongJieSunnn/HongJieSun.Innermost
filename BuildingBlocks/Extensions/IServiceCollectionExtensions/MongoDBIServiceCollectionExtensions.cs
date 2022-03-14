using Innermost.MongoDBContext;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace IServiceCollectionExtensions
{
    public static class MongoDBIServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDBSession(this IServiceCollection services)
        {
            services.AddSingleton<IClientSessionHandle>(s =>
            {
                var context = s.GetRequiredService<MongoDBContextBase>();
                return context.Client.StartSession();
            });

            return services;
        }
    }
}