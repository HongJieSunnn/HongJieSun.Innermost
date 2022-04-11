using Innermost.MusicHub.Crawler.Parsers.Singer;
using Innermost.MusicHub.Crawler.Storages.Singer;

namespace Innermost.MusicHub.Crawler.Spiders
{
    internal class SingerSpider : Spider
    {
        public SingerSpider(IOptions<SpiderOptions> options, DependenceServices services, ILogger<Spider> logger) : base(options, services, logger)
        {
        }

        protected override async Task InitializeAsync(CancellationToken stoppingToken = default)
        {
            var url = "https://y.qq.com/n/ryqq/singer/{0}";
            var requests = await SingerListService.GetRequestsBySingerListAsync(url);
            AddDataFlow(new SingerNameParser());
            AddDataFlow(new SingerDetailParser());
            AddDataFlow(new SingerStorage());
            await AddRequestsAsync(requests);
        }
    }
}
