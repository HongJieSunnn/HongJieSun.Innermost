namespace Innermost.MusicHub.Crawler.Storages
{
    internal abstract class DataFlowMongoDB : DataFlowBase
    {
        protected readonly CrawlerMongoDBContext _dbContext;
        public DataFlowMongoDB()
        {
            _dbContext = DependencyInjection.ServiceProvider.GetRequiredService<CrawlerMongoDBContext>();
        }

        public abstract override Task HandleAsync(DataFlowContext context);

        public abstract override Task InitializeAsync();
    }
}
