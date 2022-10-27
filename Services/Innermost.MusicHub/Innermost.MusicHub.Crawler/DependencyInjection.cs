using IdentityModel.Client;
using Innermost.MongoDBContext.Extensions.Microsoft.DependencyInjection;
using MediatR;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.SystemConsole.Themes;
using System.Reflection;

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
                        .MinimumLevel.Debug()
                        .Enrich.WithProperty("ApplicationContext", "Innermost.MusicHub.Crawler")
                        .Enrich.FromLogContext()
                        .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
                        .CreateLogger();
            });

            _serivces.AddMediatR(typeof(Program).GetTypeInfo().Assembly);


            _serivces.AddMongoDBContext<CrawlerMongoDBContext>(c =>
            {
                c.WithConnectionString(ConnectionString);
                c.WithDatabase("MusicHubCrawler");
            });

            _serivces.AddMongoDBContext<MusicHubMongoDBContext>(c =>
            {
                c.WithConnectionString(ConnectionString);
                c.WithDatabase("InnermostMusicHub");
            });

            _serivces.AddSingleton<TokenResponse>((service) =>
            {
                var client = new HttpClient();

                var disco = client.GetDiscoveryDocumentAsync("https://localhost:5106").GetAwaiter().GetResult();
                var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,

                    ClientId = "serviceclient",
                    ClientSecret = "service-client",
                    Scope = "tagserver"
                }).GetAwaiter().GetResult();

                return tokenResponse;
            });

            _serivces.AddHttpClient("IdentifiedHttpClient", (service, options) =>
            {
                var token = service.GetRequiredService<TokenResponse>();

                options.SetBearerToken(token.AccessToken);
            });

            if (!Builded)
            {
                Builded = true;
                ServiceProvider = _serivces.BuildServiceProvider();
            }
        }
    }
}
