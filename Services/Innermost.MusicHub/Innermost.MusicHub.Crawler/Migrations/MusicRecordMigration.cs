using IdentityModel.Client;
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
        public MusicRecordMigration()
        {
            _crawlerMongoDBContext=DependencyInjection.ServiceProvider.GetRequiredService<CrawlerMongoDBContext>();
            _context=DependencyInjection.ServiceProvider.GetRequiredService<MusicHubMongoDBContext>();
            _httpClientFactory=DependencyInjection.ServiceProvider.GetRequiredService<IHttpClientFactory>();
        }

        public async Task MigrateMusicRecordToMusicHubMongoDBContext()
        {
            var client = _httpClientFactory.CreateClient("IdentifiedHttpClient");

            var musicRecordCount=await _crawlerMongoDBContext.MusicRecords.CountDocumentsAsync(_=>true);
            List<MusicRecordEntity> musicRecordEntities;

            var tagsResponseContent =await (await client.GetAsync($"https://localhost:7075/api/tag/all")).Content.ReadAsStringAsync();

            var tagIds = new JsonSelectable(tagsResponseContent).SelectList(Selectors.JsonPath("$.[*].id")).Select(s => s.Value).ToList();
            var tagNames = new JsonSelectable(tagsResponseContent).SelectList(Selectors.JsonPath("$.[*].preferredTagName")).Select(s => s.Value).ToList();

            var tagNameIdMap = new Dictionary<string, string>();
            for (int i = 0; i < tagIds.Count; i++)
            {
                tagNameIdMap.Add(tagNames[i], tagIds[i]);
            }


            for (int i=0;i<musicRecordCount;i+=5000)
            {
                musicRecordEntities=await _crawlerMongoDBContext.MusicRecords.Find(_=>true).Skip(i).Limit(5000).ToListAsync();

                var musicRecords = musicRecordEntities.Select(async mr =>
                {
                    var album = await _crawlerMongoDBContext.Albums.Find(a => a.AlbumMid == mr.AlbumMid).FirstAsync();

                    var tagFilter = Builders<MusicTagEntity>.Filter.Eq("MusicRecordMids", mr.MusicMid);
                    var tagProjection = Builders<MusicTagEntity>.Projection.Include(mt => mt.TagName);
                    var tagNameDocuments = await _crawlerMongoDBContext.MusicTags.Find(tagFilter).Project(tagProjection).ToListAsync();
                    var tagNames = tagNameDocuments.Select(tnd => tnd.GetValue("TagName").AsString);


                    return new MusicRecord(
                    mr.MusicMid, mr.MusicId, mr.MusicName, mr.TranslatedMusicName, mr.Genre, mr.Language, mr.AlbumCoverUrl, mr.MusicUrl, mr.WikiUrl, mr.Lyric,

                    mr.SingerMids.Select((s, i) => new MusicRecordSinger(s, mr.SingerNames[i])).ToList(),

                    new MusicRecordAlbum(mr.AlbumMid, album.AlbumName, album.AlbumDescriptions, album.AlbumGenre, album.AlbumLanguage,album.SingerName,album.SingerMid, album.AlbumSongCount, album.PublishCompany, album.PublishDate),

                    mr.PublishTime,

                    tagNames.Select((tn,i)=>new TagSummary(tagNameIdMap[tn],tn)).ToList()

                    );
                });

                await _context.MusicRecords.InsertManyAsync(await Task.WhenAll(musicRecords));
            }
        }
    }
}
