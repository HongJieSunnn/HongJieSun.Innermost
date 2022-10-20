using ConfigurationExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptions<TDbContext> BuildLocalDbContextOptionsMySQL<TDbContext>(this DbContextOptionsBuilder<TDbContext> dbContextOptionsBuilder, string basePath, Type programType, Version? mysqlVersion = null)
            where TDbContext : DbContext
        {
            var config = new ConfigurationBuilder().BuildLocalConfiguration(basePath);

            dbContextOptionsBuilder.UseMySql(config.GetConnectionString("ConnectMySQL"), new MySqlServerVersion(mysqlVersion ?? new Version(5, 7)), options =>
            {
                options.MigrationsAssembly(programType.GetTypeInfo().Assembly.GetName().Name);
            });

            return dbContextOptionsBuilder.Options;
        }
    }
}
