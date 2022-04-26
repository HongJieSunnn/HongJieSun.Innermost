using Innermost.MusicHub.Domain.AggregatesModels.SingerAggregate;
using Innermost.MusicHub.Domain.AggregatesModels.SingerAggregate.Entities;

namespace Innermost.MusicHub.Crawler.Migrations
{
    internal class SingerMigration
    {
        private readonly CrawlerMongoDBContext _crawlerMongoDBContext;
        private readonly MusicHubMongoDBContext _context;
        public SingerMigration()
        {
            _crawlerMongoDBContext = DependencyInjection.ServiceProvider.GetRequiredService<CrawlerMongoDBContext>();
            _context = DependencyInjection.ServiceProvider.GetRequiredService<MusicHubMongoDBContext>();
        }

        public async Task MigrateSingerToMusicHubMongoDBContext()
        {
            var singerCount = await _crawlerMongoDBContext.Singers.CountDocumentsAsync(_ => true);

            List<SingerEntity> singerEntities;

            for (int i = 0; i < singerCount; i += 5000)
            {
                singerEntities = await _crawlerMongoDBContext.Singers.Find(_ => true).Skip(i).Limit(5000).ToListAsync();

                var singers= singerEntities.Select(async s =>
                {
                    var singerAlbumEntities = await _crawlerMongoDBContext.Albums.Find(a => a.SingerMid == s.SingerMid).ToListAsync();

                    return new Singer(
                        s.SingerMid, s.SingerId,
                        s.SingerName, s.SingerAlias,
                        s.SingerNationality, s.SingerBirthplace, s.SingerOccupation, s.SingerBirthplace, s.SingerRepresentativeWorks, s.SingerRegion, s.SingerCoverUrl,

                        singerAlbumEntities.Select(sa => new SingerAlbum(sa.AlbumMid, sa.AlbumName, sa.AlbumDescriptions, sa.AlbumGenre, sa.AlbumLanguage, sa.AlbumSongCount, sa.PublishCompany, sa.PublishDate)).ToList()
                    );
                });

                await _context.Singers.InsertManyAsync(await Task.WhenAll(singers));
            }
        }
    }
}
