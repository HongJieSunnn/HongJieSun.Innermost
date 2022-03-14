namespace Innermost.Identity.API.Factories
{
    public class InnermostIdentityDbContextFactory : IDesignTimeDbContextFactory<InnermostIdentityDbContext>
    {
        public InnermostIdentityDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory());

            var options = EntityFrameworkFactoryService.GetDbContextOptionsMySQL<InnermostIdentityDbContext>(basePath,typeof(Program));

            return new InnermostIdentityDbContext(options);
        }
    }
}
