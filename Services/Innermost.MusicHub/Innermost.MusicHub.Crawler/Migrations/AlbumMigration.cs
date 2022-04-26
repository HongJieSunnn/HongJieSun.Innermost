using Innermost.MusicHub.Domain.AggregatesModels.AlbumAggregate;
using Innermost.MusicHub.Domain.AggregatesModels.AlbumAggregate.Entities;

namespace Innermost.MusicHub.Crawler.Migrations
{
    internal class AlbumMigration
    {
        private readonly CrawlerMongoDBContext _crawlerMongoDBContext;
        private readonly MusicHubMongoDBContext _context;
        public AlbumMigration()
        {
            _crawlerMongoDBContext = DependencyInjection.ServiceProvider.GetRequiredService<CrawlerMongoDBContext>();
            _context = DependencyInjection.ServiceProvider.GetRequiredService<MusicHubMongoDBContext>();
        }

        public async Task MigrateAlbumToMusicHubMongoDBContext()
        {
            var albumsCount= await _crawlerMongoDBContext.Albums.CountDocumentsAsync(_ => true);

            List<AlbumEntity> albumEntities;

            for (int i=0; i<albumsCount; i+=5000)
            {
                albumEntities = await _crawlerMongoDBContext.Albums.Find(_ => true).Skip(i).Limit(5000).ToListAsync();

                var albums= albumEntities.Select(async a =>
                {
                    var musicRecordEnties = await _crawlerMongoDBContext.MusicRecords.Find(m => m.AlbumMid == a.AlbumMid).ToListAsync();

                    return new Album(
                        a.AlbumMid, a.AlbumId,
                        a.AlbumName, a.AlbumDescriptions,
                        a.AlbumGenre, a.AlbumLanguage,
                        a.AlbumCoverUrl,
                        a.SingerName,a.SingerMid,
                        a.AlbumSongCount,
                        a.PublishCompany, a.PublishDate,
                        musicRecordEnties.Select(mre=>new AlbumMusicRecord(
                            mre.MusicMid,mre.MusicName,mre.TranslatedMusicName,mre.Genre,mre.Language,mre.MusicUrl,
                            mre.SingerMids.Select((smid,i)=>new AlbumSinger(smid,mre.SingerNames[i])).ToList()
                            )
                        ).ToList()
                    );
                });

                await _context.Albums.InsertManyAsync(await Task.WhenAll(albums));
            }
        }
    }
}
