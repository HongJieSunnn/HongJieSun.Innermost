namespace Innermost.MusicHub.Crawler.Storages
{
    internal class CrawlerMongoDBContext : MongoDBContextBase
    {
        public IMongoCollection<SingerListEntity> SingerList { get; set; }
        public IMongoCollection<AlbumEntity> Albums { get; set; }
        public IMongoCollection<SingerEntity> Singers { get; set; }
        public IMongoCollection<MusicRecordEntity> MusicRecords { get; set; }
        public IMongoCollection<CategoryEntity> Categories { get; set; }
        public IMongoCollection<MusicListEntity> MusicLists { get; set; }
        public IMongoCollection<MusicTagEntity> MusicTags { get; set; }
        public IMongoCollection<StartMusicRecordHostTime> StartMusicRecordHostTime { get; set; }
        public CrawlerMongoDBContext(MongoDBContextConfiguration<CrawlerMongoDBContext> configuration) : base(configuration)
        {

        }
    }

    internal class StartMusicRecordHostTime
    {
        [BsonId]
        public int Id { get; set; }
        public int Times { get; set; }
        public StartMusicRecordHostTime()
        {
            Id = 1;
            Times = 0;
        }
    }
}
