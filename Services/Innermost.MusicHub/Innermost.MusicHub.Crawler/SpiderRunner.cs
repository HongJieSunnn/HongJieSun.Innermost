using DotnetSpider.Scheduler;
using DotnetSpider.Scheduler.Component;
using Innermost.MusicHub.Crawler.Spiders;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Innermost.MusicHub.Crawler
{
    internal class SpiderRunner
    {
        public static AutoResetEvent _singerListFinishEvent = new AutoResetEvent(false);


        public static Barrier SingerListBarrier = new Barrier(5, (b) => _singerListFinishEvent.Set());
        public static async Task RunSingerListSpiderAsync()
        {
            var builder = Builder.CreateDefaultBuilder<SingerListSpider>();
            builder.UseSerilog();
            builder.UseQueueDistinctBfsScheduler<HashSetDuplicateRemover>();

            await builder.Build().RunAsync();
        }

        public static async Task RunAlbumSpiderAsync()
        {
            //_singerListFinishEvent.WaitOne();
            var builder = Builder.CreateDefaultBuilder<AlbumSpider>();
            builder.UseSerilog();
            builder.UseQueueDistinctBfsScheduler<HashSetDuplicateRemover>();

            await builder.Build().RunAsync();
        }

        public static async Task RunSingerSpiderAsync()
        {
            var builder = Builder.CreateDefaultBuilder<SingerSpider>();
            builder.UseSerilog();
            builder.UseQueueDistinctBfsScheduler<HashSetDuplicateRemover>();

            await builder.Build().RunAsync();
        }
    }
}
