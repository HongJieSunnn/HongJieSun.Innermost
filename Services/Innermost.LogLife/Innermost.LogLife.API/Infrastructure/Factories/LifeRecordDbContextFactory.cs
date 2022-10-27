namespace Innemost.LogLife.API.Infrastructure.Factories
{
    public class LifeRecordDbContextFactory : IDesignTimeDbContextFactory<LifeRecordDbContext>
    {
        public LifeRecordDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory());

            var options = new DbContextOptionsBuilder<LifeRecordDbContext>().BuildLocalDbContextOptionsMySQL(basePath, typeof(Program));

            return new LifeRecordDbContext(options);
        }
    }
}
