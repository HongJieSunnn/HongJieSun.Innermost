namespace Innermost.Identity.API.Factories
{
    public class ConfigurationDbContextFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory());

            var options = EntityFrameworkFactoryService.GetDbContextOptionsMySQL<ConfigurationDbContext>(basePath,typeof(Program));

            return new ConfigurationDbContext(options, new IdentityServer4.EntityFramework.Options.ConfigurationStoreOptions());
        }
    }
}
