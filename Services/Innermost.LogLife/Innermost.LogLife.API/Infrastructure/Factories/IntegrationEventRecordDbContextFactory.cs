namespace Innemost.LogLife.API.Infrastructure.Factories
{
    public class IntegrationEventRecordDbContextFactory : IDesignTimeDbContextFactory<IntegrationEventRecordDbContext>
    {
        public IntegrationEventRecordDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory());

            var options = new DbContextOptionsBuilder<IntegrationEventRecordDbContext>().BuildLocalDbContextOptionsMySQL(basePath, typeof(Program));

            return new IntegrationEventRecordDbContext(options);
        }
    }
}
