namespace Innermost.Identity.API.Factories
{
    public class InnermostIdentityDbContextFactory : IDesignTimeDbContextFactory<InnermostIdentityDbContext>
    {
        public InnermostIdentityDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory());

            var options = new DbContextOptionsBuilder<InnermostIdentityDbContext>().BuildLocalDbContextOptionsMySQL(basePath, typeof(Program));

            return new InnermostIdentityDbContext(options);
        }
    }
}
