using Innermost.MusicHub.Domain.AggregatesModels.SingerAggregate;
using Innermost.MusicHub.Domain.AggregatesModels.SingerAggregate.Entities;

namespace Innermost.MusicHub.Crawler.Migrations
{
    internal class SingerMigration
    {
        private readonly CrawlerMongoDBContext _crawlerMongoDBContext;
        private readonly MusicHubMongoDBContext _context;
        private readonly Serilog.ILogger _logger;
        public SingerMigration()
        {
            _crawlerMongoDBContext = DependencyInjection.ServiceProvider.GetRequiredService<CrawlerMongoDBContext>();
            _context = DependencyInjection.ServiceProvider.GetRequiredService<MusicHubMongoDBContext>();
            _logger= DependencyInjection.ServiceProvider.GetRequiredService<Serilog.ILogger>();
        }

        public async Task MigrateSingerToMusicHubMongoDBContext()
        {
            _logger.Information("Start migrate singers");
            var singerCount = await _crawlerMongoDBContext.Singers.CountDocumentsAsync(_ => true);

            List<SingerEntity> singerEntities;
            IEnumerable<Task<Singer>>? singers=null;
            for (int i = 0; i < singerCount; i += 1000)
            {
                _logger.Information($"Migrate singers from {i} to {i+1000}");
                singerEntities = await _crawlerMongoDBContext.Singers.Find(_ => true).Skip(i).Limit(1000).ToListAsync();
                _logger.Information($"Get singer entities count {singerEntities.Count}");

                singers = singerEntities.Select(async s =>
                {
                    var singerAlbumEntities = await _crawlerMongoDBContext.Albums.Find(a => a.SingerMid == s.SingerMid).ToListAsync();
                    singerAlbumEntities=singerAlbumEntities.OrderByDescending(a => a.PublishDate).ToList();
                    return new Singer(
                        s.SingerMid, s.SingerId,
                        s.SingerName, s.SingerAlias,
                        s.SingerNationality, s.SingerBirthplace, s.SingerOccupation, s.SingerBirthplace, s.SingerRepresentativeWorks, s.SingerRegion, s.SingerCoverUrl,

                        singerAlbumEntities.Select(sa => new SingerAlbum(sa.AlbumMid, sa.AlbumName, sa.AlbumDescriptions, sa.AlbumGenre, sa.AlbumLanguage,sa.AlbumCoverUrl, sa.AlbumSongCount, sa.PublishCompany, sa.PublishDate)).ToList()
                    );
                });
                _logger.Information("Start insert singers");
                await _context.Singers.InsertManyAsync(await Task.WhenAll(singers));
            }
        }
    }
}
