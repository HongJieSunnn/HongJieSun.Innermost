using Innermost.MongoDBContext;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Innermost.IServiceCollectionExtensions
{
    public static class MongoDBIServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDBSession(this IServiceCollection services)
        {
            services.AddScoped<IClientSessionHandle>(s =>
            {
                var context = s.GetRequiredService<MongoDBContextBase>();
                return context.Client.StartSession();
            });

            return services;
        }
    }
}