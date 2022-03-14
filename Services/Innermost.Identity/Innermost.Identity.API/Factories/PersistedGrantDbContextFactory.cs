namespace Innermost.Identity.API.Factories
{
    public class PersistedGrantDbContextFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
    {
        public PersistedGrantDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory());

            var options = EntityFrameworkFactoryService.GetDbContextOptionsMySQL<PersistedGrantDbContext>(basePath,typeof(Program));

            return new PersistedGrantDbContext(options, new IdentityServer4.EntityFramework.Options.OperationalStoreOptions());
        }
    }
}
