using Innermost.MusicHub.Crawler.Parsers.MusicTag;

namespace Innermost.MusicHub.Crawler.Spiders
{
    internal class MusicTagSpider : Spider
    {
        public MusicTagSpider(IOptions<SpiderOptions> options, DependenceServices services, ILogger<Spider> logger) : base(options, services, logger)
        {
        }

        protected override async Task InitializeAsync(CancellationToken stoppingToken = default)
        {
            AddDataFlow(new MusicCategoriesParser());
            AddDataFlow(new MusicListParser());
            AddDataFlow(new MusicListDetailParser());

            await AddRequestsAsync("http://localhost:3200/getSongListCategories");
        }
    }
}
