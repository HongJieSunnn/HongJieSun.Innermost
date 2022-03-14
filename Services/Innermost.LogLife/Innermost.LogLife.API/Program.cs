using Innemost.LogLife.API.Infrastructure.SeedDatas;


IConfiguration configuration = Program.GetConfiguration();

Log.Logger = CreateSerilogLogger(configuration);

try
{
    Log.Information("Configuring web host ({ApplicationContext})...", Program.AppName);
    var host = CreateWebHostBuilder(configuration, args);

    Log.Information("Applying migrations ({ApplicationContext})...", Program.AppName);
    host.MigrateDbContext<LifeRecordDbContext>((context, service) =>
    {
        new LifeRecordDbContextSeed()
            .SeedAsync(context, configuration)
            .Wait();
    })
        .MigrateDbContext<IntegrationEventRecordDbContext>((_, __) => { });

    Log.Information("Starting web host ({ApplicationContext})...", Program.AppName);
    host.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", Program.AppName);
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

#pragma warning disable CS0618 // 类型或成员已过时
IWebHost CreateWebHostBuilder(IConfiguration configuration, string[] args) => WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .CaptureStartupErrors(false)
                .ConfigureAppConfiguration(c => c.AddConfiguration(configuration))
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseSerilog()
                .Build();
#pragma warning restore CS0618 // 类型或成员已过时

Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
{
    var seqServerUrl = configuration["Serilog:SeqServerUrl"];
    var logstashUrl = configuration["Serilog:LogstashgUrl"];
    return new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .Enrich.WithProperty("ApplicationContext", Program.AppName)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

public partial class Program
{
    public static string Namespace = typeof(Startup).Namespace;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

    public static IConfiguration GetConfiguration()
    {
        var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                        .AddEnvironmentVariables();//no environmentvariables in this service

        var config = builder.Build();

        return config;
    }
}