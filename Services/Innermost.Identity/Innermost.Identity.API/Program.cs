string Namespace = typeof(Startup).Namespace;
string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

var configuration = GetConfiguration();

Log.Logger = CreateSerilogLogger(configuration);

try
{
    Log.Information("Configuring web host ({ApplicationContext})...", AppName);
    var host = CreateHostBuilder(args, configuration);

    Log.Information("Applying migrations ({ApplicationContext})...", AppName);
    host.MigrateDbContext<PersistedGrantDbContext>((_, __) => { })
        .MigrateDbContext<InnermostIdentityDbContext>((dbContext, services) =>
        {
            var userManager= services.GetRequiredService<UserManager<InnermostUser>>();
            new InnermostIdentityDbContextSeed()
                .SeedAsync(dbContext,userManager, configuration)
                .Wait();
        })
        .MigrateDbContext<ConfigurationDbContext>((dbContext, services) =>
        {
            //MigrateDbContext �� IWebHost ����չ��������ô�ú�������ʹ�õ������� IWebHost ʵ���е����ݣ�����Ϳ���ʹ��ʵ�� host �е�����
            //service ֻ�Ǹ��βΣ���MigrateDbContext��ͨ�� IWebHost ʵ����ȡ�� IServiceProvider ��ʵ������ʼ�� service������ͨ�� service ��ȡ��Ӧ�� DbContext ʵ��
            //Ȼ�� MigrateDbContext �� Invoke ���Ǵ���� seeder �������ʽ��������ʼ�����ݿ�Ĺ������� MigrateDbContext ������������Ҫ����������
            new ConfigurationDbContextSeed()
                .SeedAsync(dbContext, configuration)
                .Wait();
        });

    Log.Information("Starting web host ({ApplicationContext})...", AppName);
    host.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
    return 1;
}
finally
{
    Log.CloseAndFlush();
}



#pragma warning disable CS0618 // ���ͻ��Ա�ѹ�ʱ
IWebHost CreateHostBuilder(string[] args, IConfiguration configuration) => 
    WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .CaptureStartupErrors(false)
                .ConfigureAppConfiguration(c => c.AddConfiguration(configuration))
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseSerilog()
                .Build();
#pragma warning restore CS0618 // ���ͻ��Ա�ѹ�ʱ


Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
{
    var seqServerUrl = configuration["Serilog:SeqServerUrl"];
    var logstashUrl = configuration["Serilog:LogstashgUrl"];
    return new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .Enrich.WithProperty("ApplicationContext", AppName)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://localhost:8080" : logstashUrl)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    var config = builder.Build();

    return builder.Build();
}
