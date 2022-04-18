using DotnetSpider.Proxy;
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
            var builder = Builder.CreateDefaultBuilder<SingerListSpider>(options=>
            {
                options.Batch = 30;
                options.Speed = 30;
            });

            builder.UseSerilog();
            builder.UseQueueDistinctBfsScheduler<HashSetDuplicateRemover>();
            //builder.UseProxy<EmptyProxySupplier, DefaultProxyValidator>(x =>
            //{
            //    x.ProxySupplierUrl = "http://localhost:10808";
            //});//Not work

            await builder.Build().RunAsync();
        }

        public static async Task RunAlbumSpiderAsync()
        {
            //_singerListFinishEvent.WaitOne();
            var builder = Builder.CreateDefaultBuilder<AlbumSpider>(options =>
            {
                options.Batch = 30;
                options.Speed = 30;
            });

            builder.UseSerilog();
            builder.UseQueueDistinctBfsScheduler<HashSetDuplicateRemover>();
            //builder.UseProxy<FiddlerProxySupplier, DefaultProxyValidator>(x =>
            //{
            //    x.ProxyTestUrl = "http://localhost:10808";
            //});

            await builder.Build().RunAsync();
        }

        public static async Task RunSingerSpiderAsync()
        {
            var builder = Builder.CreateDefaultBuilder<SingerSpider>(options =>
            {
                options.Batch = 30;
                options.Speed = 30;
            });
            builder.UseSerilog();
            builder.UseQueueDistinctBfsScheduler<HashSetDuplicateRemover>();
            //builder.UseProxy<FiddlerProxySupplier, DefaultProxyValidator>(x =>
            //{
            //    x.ProxyTestUrl = "http://localhost:10808";
            //});

            await builder.Build().RunAsync();
        }

        public static async Task RunMusicRecordSpiderAsync()
        {
            while (!AlbumService.IsFinished())
            {
                var builder = Builder.CreateDefaultBuilder<MusicRecordSpider>(options =>
                {
                    options.Batch = 22;
                    options.Speed = 20;
                    options.RetriedTimes = 5;
                });
                builder.UseSerilog();
                builder.UseQueueDistinctBfsScheduler<HashSetDuplicateRemover>();
                
                await builder.Build().RunAsync();
            }
        }

        public static async Task RunMusicTagSpiderAsync()
        {
            var builder = Builder.CreateDefaultBuilder<MusicTagSpider>(options =>
            {
                options.Batch = 10;
                options.Speed = 10;
            });
            builder.UseSerilog();
            builder.UseQueueDistinctBfsScheduler<HashSetDuplicateRemover>();

            await builder.Build().RunAsync();
        }
    }
}
