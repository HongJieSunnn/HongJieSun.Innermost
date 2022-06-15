using Innermost.MusicHub.Crawler.Parsers.MusicRecord;

namespace Innermost.MusicHub.Crawler.Spiders
{
    internal class MusicRecordSpider : Spider
    {
        public MusicRecordSpider(IOptions<SpiderOptions> options, DependenceServices services, ILogger<Spider> logger) : base(options, services, logger)
        {
        }

        protected override async Task InitializeAsync(CancellationToken stoppingToken = default)
        {
            var url = "http://localhost:3200/getSongInfo?songmid={0}";
            var requests = await AlbumService.GetRequestsByAlbumSongsAsync(url);

            AddDataFlow(new MusicDetailParser());

            await AddRequestsAsync(requests);
        }
    }
}
