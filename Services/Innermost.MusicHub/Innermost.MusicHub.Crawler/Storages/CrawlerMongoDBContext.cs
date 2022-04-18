using Innermost.MongoDBContext.Configurations;
using MongoDB.Driver;

namespace Innermost.MusicHub.Crawler.Storages
{
    internal class CrawlerMongoDBContext : MongoDBContextBase
    {
        public IMongoCollection<SingerListEntity> SingerList { get; set; }
        public IMongoCollection<AlbumEntity> Albums { get; set; }
        public IMongoCollection<SingerEntity> Singers { get; set; }
        public CrawlerMongoDBContext(MongoDBContextConfiguration<CrawlerMongoDBContext> configuration) : base(configuration)
        {

        }
    }
}
