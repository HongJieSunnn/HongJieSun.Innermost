using Serilog;

IConfiguration configuration = GetConfiguration();
Log.Logger = CreateSerilogLogger(configuration);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddTagSServer(config =>
    {
        config.WithConnectionString(builder.Configuration.GetConnectionString("MongoDB"));
        config.WithDatabase("Innermost.TagServer");
    });

builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory(container =>
    {
        container.Populate(builder.Services);
        container.RegisterTagSMicroservicesServerTypes();
    }))//Use Autofac to Provider IServiceProvider.
    .UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

ConfigureTagSServer(app);

app.Run();


void ConfigureTagSServer(IApplicationBuilder builder)
{
    builder.AddReferrerDiscriminator<LifeRecordReferrer>();
    builder.AddLocationIndexFroReferrer("BaiduPOI");
}

Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
{
    var seqServerUrl = configuration["Serilog:SeqServerUrl"];
    var logstashUrl = configuration["Serilog:LogstashgUrl"];
    return new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .Enrich.WithProperty("ApplicationContext", AppName)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

partial class Program
{
    public static string Namespace => typeof(Program).Namespace;
    public static string AppName => Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
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