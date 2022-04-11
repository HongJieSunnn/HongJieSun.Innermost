using Innermost.MongoDBContext.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.SystemConsole.Themes;

namespace Innermost.MusicHub.Crawler
{
    internal class DependencyInjection
    {
        private static bool Builded = false;
        private readonly static IServiceCollection _serivces = new ServiceCollection();
        private const string ConnectionString = "mongodb://root:hong456..@localhost:27018,localhost:27019,localhost:27020/?authSource=admin&replicaSet=mongo&readPreference=secondaryPreferred&appname=InnermostMusicHubCrawler";

        public static IServiceProvider ServiceProvider { get; private set; }

        public static void BuildServiceProvider()
        {
            _serivces.AddSingleton<Serilog.ILogger, Logger>((services) =>
            {
                return new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .Enrich.WithProperty("ApplicationContext", "Innermost.MusicHub.Crawler")
                        .Enrich.FromLogContext()
                        .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
                        .CreateLogger();
            });


            _serivces.AddMongoDBContext<CrawlerMongoDBContext>(c =>
            {
                c.WithConnectionString(ConnectionString);
                c.WithDatabase("MusicHubCrawler");
            });

            _serivces.AddHttpClient();

            if (!Builded)
            {
                Builded = true;
                ServiceProvider = _serivces.BuildServiceProvider();
            }
        }
    }
}
