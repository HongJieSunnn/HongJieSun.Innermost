namespace Innermost.MusicHub.Crawler.Parsers.Album
{
    internal class AlbumDetailParser : DataParser
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AlbumDetailParser()
        {
            _httpClientFactory = DependencyInjection.ServiceProvider.GetRequiredService<IHttpClientFactory>();
        }
        public override Task InitializeAsync()
        {
            AddRequiredValidator(@"localhost:3200/getAlbumInfo\?albummid=\w{14}");
            return Task.CompletedTask;
        }

        protected override async Task ParseAsync(DataFlowContext context)
        {
            var albumId = context.Selectable.JsonPath("$.response.data.id").Value;
            var client = _httpClientFactory.CreateClient();
            var coverResponseTask = client.GetAsync($"http://imgcache.qq.com/music/photo/album_800/20/800_albumpic_{albumId}_0.jpg");//get album cover

            var albumMid = context.Selectable.JsonPath("$.response.data.mid").Value;
            var albumName= context.Selectable.JsonPath("$.response.data.name").Value;
            var singerMid= context.Selectable.JsonPath("$.response.data.singermid").Value;
            var singerName= context.Selectable.JsonPath("$.response.data.singername").Value;
            var albumDesc= context.Selectable.JsonPath("$.response.data.desc").Value;
            var albumGenre = context.Selectable.JsonPath("$.response.data.genre").Value;
            var albumLan= context.Selectable.JsonPath("$.response.data.lan").Value;
            var albumSongCount= int.Parse(context.Selectable.JsonPath("$.response.data.total_song_num").Value);
            var publishCompany= context.Selectable.JsonPath("$.response.data.company").Value;
            var publishDate= context.Selectable.JsonPath("$.response.data.aDate").Value;
            var albumMusicList = context.Selectable.SelectList(Selectors.JsonPath("$.response.data.list.[*].songmid")).Select(l=>l.Value).ToList();
            var converResponse = await coverResponseTask;
            var coverUrl = converResponse!.RequestMessage!.RequestUri!.ToString();//the album cover will be responsed by a new uri which can be got by HttpResponseMessage.RequestMessage

            var album = new AlbumEntity(int.Parse(albumId),albumMid, albumName, albumDesc, albumGenre, albumLan, coverUrl, albumSongCount, publishCompany, publishDate, singerName, singerMid, albumMusicList);

            context.AddData("album", album);
        }
    }
}
