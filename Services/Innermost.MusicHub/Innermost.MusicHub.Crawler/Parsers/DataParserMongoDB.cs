namespace Innermost.MusicHub.Crawler.Parsers
{
    internal abstract class DataParserMongoDB:DataParser
    {
        protected readonly CrawlerMongoDBContext _dbContext;
        public DataParserMongoDB()
        {
            _dbContext=DependencyInjection.ServiceProvider.GetRequiredService<CrawlerMongoDBContext>();
        }

    }
}
