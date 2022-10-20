using Innermost.MusicHub.Domain.AggregatesModels.AlbumAggregate;
using Innermost.MusicHub.Domain.AggregatesModels.AlbumAggregate.Entities;

namespace Innermost.MusicHub.Crawler.Migrations
{
    internal class AlbumMigration
    {
        private readonly CrawlerMongoDBContext _crawlerMongoDBContext;
        private readonly MusicHubMongoDBContext _context;
        private readonly Serilog.ILogger _logger;
        public AlbumMigration()
        {
            _crawlerMongoDBContext = DependencyInjection.ServiceProvider.GetRequiredService<CrawlerMongoDBContext>();
            _context = DependencyInjection.ServiceProvider.GetRequiredService<MusicHubMongoDBContext>();
            _logger = DependencyInjection.ServiceProvider.GetRequiredService<Serilog.ILogger>();
        }

        public async Task MigrateAlbumToMusicHubMongoDBContext()
        {
            _logger.Information($"Start migrate albums {DateTime.Now}");
            var albumsCount = await _crawlerMongoDBContext.Albums.CountDocumentsAsync(_ => true);

            List<AlbumEntity> albumEntities;

            for (int i = 0; i < albumsCount; i += 500)
            {
                _logger.Information($"Migrate singers from {i} to {i + 500} {DateTime.Now}");
                albumEntities = await _crawlerMongoDBContext.Albums.Find(_ => true).Skip(i).Limit(500).ToListAsync();

                var filter = Builders<MusicRecordEntity>.Filter.In(m => m.AlbumMid, albumEntities.Select(a => a.AlbumMid));
                var musicRecordEnties = await _crawlerMongoDBContext.MusicRecords.Find(filter).ToListAsync();

                var albums = albumEntities.Select(a =>
                {
                    return new Album(
                        a.AlbumMid, a.AlbumId,
                        a.AlbumName, a.AlbumDescriptions,
                        a.AlbumGenre, a.AlbumLanguage,
                        a.AlbumCoverUrl,
                        a.SingerName, a.SingerMid,
                        a.AlbumSongCount,
                        a.PublishCompany, a.PublishDate,
                        musicRecordEnties.Where(m => m.AlbumMid == a.AlbumMid).Select(mre => new AlbumMusicRecord(
                            mre.MusicMid, mre.MusicName, mre.TranslatedMusicName, mre.Genre, mre.Language, mre.MusicUrl,
                            mre.SingerMids.Select((smid, i) => new AlbumSinger(smid, mre.SingerNames[i])).ToList()
                            )
                        ).ToList()
                    );
                });
                _logger.Information($"Start insert albums {DateTime.Now}");
                try
                {
                    await _context.Albums.InsertManyAsync(albums);
                }
                catch (Exception ex)
                {
                    var dupAlbumfilter = Builders<Album>.Filter.In(a => a.AlbumMid, albums.Select(a => a.AlbumMid));
                    var dupAlbums = await _context.Albums.Find(dupAlbumfilter).ToListAsync();
                    albums = albums.Where(a => !dupAlbums.Select(da => da.AlbumMid).Contains(a.AlbumMid));
                    if (albums != null && albums.Any())
                        await _context.Albums.InsertManyAsync(albums);
                }
            }
        }
    }
}
