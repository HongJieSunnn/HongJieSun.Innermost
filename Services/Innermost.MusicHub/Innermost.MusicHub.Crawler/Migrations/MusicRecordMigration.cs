using IdentityModel.Client;
using Innermost.MusicHub.Domain.AggregatesModels.AlbumAggregate;
using Innermost.MusicHub.Domain.AggregatesModels.AlbumAggregate.Entities;
using Innermost.MusicHub.Domain.AggregatesModels.MusicRecordAggregate;
using Innermost.MusicHub.Domain.AggregatesModels.MusicRecordAggregate.Entities;
using System.Text.Json.Nodes;
using TagS.Microservices.Client.Models;

namespace Innermost.MusicHub.Crawler.Migrations
{
    internal class MusicRecordMigration
    {
        private readonly CrawlerMongoDBContext _crawlerMongoDBContext;
        private readonly MusicHubMongoDBContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Serilog.ILogger _logger;
        public MusicRecordMigration()
        {
            _crawlerMongoDBContext=DependencyInjection.ServiceProvider.GetRequiredService<CrawlerMongoDBContext>();
            _context=DependencyInjection.ServiceProvider.GetRequiredService<MusicHubMongoDBContext>();
            _httpClientFactory=DependencyInjection.ServiceProvider.GetRequiredService<IHttpClientFactory>();
            _logger = DependencyInjection.ServiceProvider.GetRequiredService<Serilog.ILogger>();
        }

        public async Task MigrateMusicRecordToMusicHubMongoDBContext()
        {
            _logger.Information($"Start migrate musicRecords {DateTime.Now}");
            var client = _httpClientFactory.CreateClient("IdentifiedHttpClient");

            var musicRecordCountTask=_crawlerMongoDBContext.MusicRecords.CountDocumentsAsync(_=>true);
            List<MusicRecordEntity> musicRecordEntities;

            var tagsResponseContent =await (await client.GetAsync($"https://localhost:7075/api/tag/all")).Content.ReadAsStringAsync();

            var tagIds = new JsonSelectable(tagsResponseContent).SelectList(Selectors.JsonPath("$.[*].id")).Select(s => s.Value).ToList();
            var tagNames = new JsonSelectable(tagsResponseContent).SelectList(Selectors.JsonPath("$.[*].preferredTagName")).Select(s => s.Value).ToList();

            var tagNameIdMap = new Dictionary<string, string>();
            for (int i = 0; i < tagIds.Count; i++)
            {
                tagNameIdMap.Add(tagNames[i], tagIds[i]);
            }

            var musicTags = await _crawlerMongoDBContext.MusicTags.Find(_ => true).ToListAsync();

            var musicRecordCount = await musicRecordCountTask;
            for (int i=278000;i<musicRecordCount;i+=500)
            {
                _logger.Information($"Migrate musicRecords from {i} to {i + 500} {DateTime.Now}");
                musicRecordEntities =await _crawlerMongoDBContext.MusicRecords.Find(mr=>mr.AlbumMid!="").Skip(i).Limit(500).ToListAsync();


                var albumFilter = Builders<AlbumEntity>.Filter.In(a => a.AlbumMid, musicRecordEntities.Select(m => m.AlbumMid));
                var albums = await _crawlerMongoDBContext.Albums.Find(albumFilter).ToListAsync();

                //Inexplicably, some albummid don't have the corresponding album, but they crawled according to the album, speechless
                musicRecordEntities = musicRecordEntities.Where(mr => albums.Select(a => a.AlbumMid).Contains(mr.AlbumMid)).ToList();

                var musicRecords = musicRecordEntities.Select(mr =>
                {
                    
                    var album = albums.FirstOrDefault(a => a.AlbumMid == mr.AlbumMid);
                    
                    var tagSummaries = musicTags.Where(mt => mt.MusicRecordMids.Contains(mr.MusicMid)).Select(mt => new TagSummary(tagNameIdMap[mt.TagName], mt.TagName)).ToList();

                    return new MusicRecord(
                    mr.MusicMid, mr.MusicId, mr.MusicName, mr.TranslatedMusicName,mr.Introduction, mr.Genre, mr.Language, mr.AlbumCoverUrl, mr.MusicUrl, mr.WikiUrl, mr.Lyric,

                    mr.SingerMids.Select((s, i) => new MusicRecordSinger(s, mr.SingerNames[i])).ToList(),

                    new MusicRecordAlbum(mr.AlbumMid, album.AlbumName, album.AlbumDescriptions, album.AlbumGenre, album.AlbumLanguage,album.SingerName,album.SingerMid, album.AlbumSongCount, album.PublishCompany, album.PublishDate),

                    mr.PublishTime,

                    tagSummaries

                    );
                });
                _logger.Information($"Start insert musicRecords {DateTime.Now}");
                try
                {
                    await _context.MusicRecords.InsertManyAsync(musicRecords);
                }
                catch (Exception ex)
                {
                    var dupfilter = Builders<MusicRecord>.Filter.In(m => m.MusicMid, musicRecords.Select(m=>m.MusicMid));
                    var dupMusicRecords = await _context.MusicRecords.Find(dupfilter).ToListAsync();
                    musicRecords = musicRecords.Where(m => !dupMusicRecords.Select(dm => dm.MusicMid).Contains(m.MusicMid));
                    if (musicRecords != null && musicRecords.Any())
                        await _context.MusicRecords.InsertManyAsync(musicRecords);
                }
            }
        }

        public async Task MigrateMusicRecordAddIntroductionAsync()
        {
            var hasIntroductionCount = await _crawlerMongoDBContext.MusicRecords.CountDocumentsAsync(m => m.Introduction != null);
            for (int i = 0; i < hasIntroductionCount; i+=500)
            {
                _logger.Information($"Update musicRecords from {i} to {i + 500} {DateTime.Now}");

                var crawledMusicRecords = await _crawlerMongoDBContext.MusicRecords.Find(m => m.Introduction != null).Skip(i).Limit(500).ToListAsync();

                var filters = crawledMusicRecords.Select(cmr=> Builders<MusicRecord>.Filter.Eq(m=>m.MusicMid,cmr.MusicMid)).ToArray();
                var updateDefinitions = crawledMusicRecords.Select(cmr => Builders<MusicRecord>.Update.Set(m => m.Introduction, cmr.Introduction)).ToArray();

                for(int j=0;j< crawledMusicRecords.Count;++j)
                {
                    await _context.MusicRecords.UpdateOneAsync(filters[j], updateDefinitions[j]);
                }
            }
        }

        public async Task MigrateMusicRecordUpdateTagIdAsync()
        {
            _logger.Information($"Start update musicRecords {DateTime.Now}");
            var client = _httpClientFactory.CreateClient("IdentifiedHttpClient");

            var tagsResponseContent = await (await client.GetAsync($"https://localhost:7075/api/tag/all")).Content.ReadAsStringAsync();

            var tagIds = new JsonSelectable(tagsResponseContent).SelectList(Selectors.JsonPath("$.[*].id")).Select(s => s.Value).ToList();
            var tagNames = new JsonSelectable(tagsResponseContent).SelectList(Selectors.JsonPath("$.[*].preferredTagName")).Select(s => s.Value).ToList();
            _logger.Information($"Complete get tags {DateTime.Now}");
            for (int i=0;i<tagIds.Count;++i)
            {
                _logger.Information($"Start update tag {tagNames[i]} {DateTime.Now}");
                var filter = Builders<MusicRecord>.Filter.Eq("Tags.TagName", tagNames[i]) &
                    Builders<MusicRecord>.Filter.ElemMatch("Tags", Builders<TagSummary>.Filter.Eq(t=>t.TagName,tagNames[i]));
                var update = Builders<MusicRecord>.Update.Set("Tags.$.TagId", ObjectId.Parse(tagIds[i]));

                await _context.MusicRecords.UpdateManyAsync(filter, update);
            }
        }
    }
}
