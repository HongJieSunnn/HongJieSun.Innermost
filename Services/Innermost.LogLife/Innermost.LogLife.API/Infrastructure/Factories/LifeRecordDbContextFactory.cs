﻿namespace Innemost.LogLife.API.Infrastructure.Factories
{
    public class LifeRecordDbContextFactory : IDesignTimeDbContextFactory<LifeRecordDbContext>
    {
        public LifeRecordDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory());

            var options = EntityFrameworkFactoryService.GetDbContextOptionsMySQL<LifeRecordDbContext>(basePath, typeof(Program));

            return new LifeRecordDbContext(options);
        }
    }
}
