namespace Innermost.MusicHub.Crawler.Parsers.Album
{
    internal class AlbumMidParser : DataParser
    {
        public override Task InitializeAsync()
        {
            AddRequiredValidator(@"localhost:3200/getSingerAlbum\?singermid=\w{14}&limit=1000");
            return Task.CompletedTask;
        }

        protected override Task ParseAsync(DataFlowContext context)
        {
            var albumMids = context.Selectable.SelectList(Selectors.JsonPath("$.response.singer.data.albumList.[*].albumMid")).Select(a => a.Value);

            context.AddFollowRequests(GenerateAlbumDetailUrls(albumMids));

            return Task.CompletedTask;
        }

        private IEnumerable<Request> GenerateAlbumDetailUrls(IEnumerable<string> albumMids)
        {
            return albumMids.Select(aMid =>
            {
                var request = new Request($"http://localhost:3200/getAlbumInfo?albummid={aMid}");
                request.Headers.UserAgent = UserAgentPool.GetRandomUserAgent();
                return request;
            });
        }
    }
}
