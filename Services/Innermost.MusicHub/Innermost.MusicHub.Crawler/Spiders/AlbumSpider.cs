using Innermost.MusicHub.Crawler.Parsers.Album;
using Innermost.MusicHub.Crawler.Storages.Album;

namespace Innermost.MusicHub.Crawler.Spiders
{
    internal class AlbumSpider : Spider
    {
        public AlbumSpider(IOptions<SpiderOptions> options, DependenceServices services, ILogger<Spider> logger) : base(options, services, logger)
        {
        }

        protected override async Task InitializeAsync(CancellationToken stoppingToken = default)
        {
            var url = "http://localhost:3200/getSingerAlbum?singermid={0}&limit=1000";
            var requests = await SingerListService.GetRequestsBySingerListAsync(url);

            AddDataFlow(new AlbumMidParser());
            AddDataFlow(new AlbumDetailParser());
            AddDataFlow(new AlbumStorage());

            await AddRequestsAsync(requests);
        }
    }
}
