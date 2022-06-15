namespace Innermost.MusicHub.Crawler.Parsers.MusicRecord
{
    internal class MusicDetailParser : DataParserMongoDB
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public MusicDetailParser()
        {
            _httpClientFactory = DependencyInjection.ServiceProvider.GetRequiredService<IHttpClientFactory>();
        }
        public override Task InitializeAsync()
        {
            AddRequiredValidator(@"localhost:3200/getSongInfo\?songmid=\w{14}");
            return Task.CompletedTask;
        }

        protected override async Task ParseAsync(DataFlowContext context)
        {
            AlbumService.TakeOne();
            if (context.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(300));
                await Task.CompletedTask;
                return;
            }

            var musicId = context.Selectable.JsonPath("$.response.songinfo.data.track_info.id").Value;
            var musicMid = context.Selectable.JsonPath("$.response.songinfo.data.track_info.mid").Value;

            var client = _httpClientFactory.CreateClient();
            if (context.Request.Headers.UserAgent is not null)
                client.DefaultRequestHeaders.UserAgent.ParseAdd(context.Request.Headers.UserAgent);
            var responseLyricTask = client.GetAsync($"http://localhost:3200/getLyric?songmid={musicMid}");

            var musicName = context.Selectable.JsonPath("$.response.songinfo.data.track_info.name").Value;
            var genre = context.Selectable.JsonPath("$.response.songinfo.data.info.genre.content.[0].value")?.Value;
            var introduction = context.Selectable.JsonPath("$.response.songinfo.data.info.intro.content.[0].value")?.Value;
            var language = context.Selectable.JsonPath("$.response.songinfo.data.info.lan.content.[0].value")?.Value;
            var publishTime = context.Selectable.JsonPath("$.response.songinfo.data.info.pub_time.content.[0].value")?.Value;
            var singerMids = context.Selectable.SelectList(Selectors.JsonPath("$.response.songinfo.data.track_info.singer.[*].mid")).Select(s => s.Value).ToList();
            var singerNames = context.Selectable.SelectList(Selectors.JsonPath("$.response.songinfo.data.track_info.singer.[*].name")).Select(s => s.Value).ToList();
            var albumMid = context.Selectable.JsonPath("$.response.songinfo.data.track_info.album.mid").Value;
            var wikiUrl = context.Selectable.JsonPath("$.response.songinfo.data.extras.wikiurl")?.Value;
            var transName = context.Selectable.JsonPath("$.response.songinfo.data.extras.transname")?.Value;
            var musicUrl = @$"https://y.qq.com/n/ryqq/songDetail/{musicMid}";

            context.Request.Properties.TryGetValue("albumCoverUrl", out var albumCoverUrl);
            if (albumCoverUrl is null)
            {
                var albumFileter = Builders<AlbumEntity>.Filter.Eq("_id", albumMid);
                var album = _dbContext.Albums.Find(albumFileter).First();

                albumCoverUrl = album.AlbumCoverUrl;
            }

            var responseLyric = await responseLyricTask;
            var lyric = new JsonSelectable(await responseLyric.Content.ReadAsStringAsync()).JsonPath("$.response.lyric")?.Value ?? "";

            var musicRecord = new MusicRecordEntity(musicMid, int.Parse(musicId), musicName, transName,introduction, genre, language, albumMid, (string)albumCoverUrl!, musicUrl, wikiUrl, lyric, singerMids, singerNames, publishTime);

            await _dbContext.MusicRecords.InsertOneAsync(musicRecord);
        }
    }
}
