using Innermost.MusicHub.Crawler;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.WithProperty("ApplicationContext", "Innermost.MusicHub.Crawler")
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Literate).WriteTo.File($"logs/spider.log")
                .CreateLogger();

DependencyInjection.BuildServiceProvider();

//await SpiderRunner.RunSingerListSpiderAsync();

//await SpiderRunner.RunAlbumSpiderAsync();

//await SpiderRunner.RunSingerSpiderAsync();

await SpiderRunner.RunMusicRecordSpiderAsync();

//await SpiderRunner.RunMusicTagSpiderAsync();