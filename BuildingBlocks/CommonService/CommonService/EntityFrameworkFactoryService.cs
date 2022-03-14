using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace CommonService
{
    public class EntityFrameworkFactoryService
    {
        public static DbContextOptions<TContext> GetDbContextOptionsMySQL<TContext>(string basePath, Type programType) where TContext : DbContext
        {
            var config = ConfigurationService.GetConfiguration(basePath);

            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            optionsBuilder.UseMySql(config.GetConnectionString("ConnectMySQL"), new MySqlServerVersion(new Version(5, 7)), options =>
            {
                options.MigrationsAssembly(programType.GetTypeInfo().Assembly.GetName().Name);
            });

            return optionsBuilder.Options;
        }
    }
}
