using Autofac.Extensions.DependencyInjection;
using Innermost.Intelligence.API.LifeRecord;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ReactApp.HttpAggregator.Infrastructure;
using ReactApp.HttpAggregator.Services.Daily;
using ReactApp.HttpAggregator.Services.Intelligence;
using ReactApp.HttpAggregator.Services.LogLife;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

IConfiguration configuration = GetConfiguration();
Log.Logger = CreateSerilogLogger(configuration);

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureAppConfiguration(c => c.AddConfiguration(configuration))
    .UseContentRoot(Directory.GetCurrentDirectory())
    .UseSerilog();

// Add services to the container.

builder.Services.AddSignalR();

builder.Services
    .AddCustomAuthentication(configuration)
    .AddReactAppHttpAggregatorConfigurations(configuration)
    .AddReactAppHttpAggregatorHttpClients()
    .AddReactAppHttpAggregatorGrpcServices()
    .AddIdentityService()
    .AddReactAppHttpAggregatorGrpcClients(configuration)
    .AddCustomCORS();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("ReactApp");

if (configuration.GetValue<bool>("UseHttpsRedirection"))
    app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
{
    return new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .Enrich.WithProperty("ApplicationContext", AppName)
        .Enrich.FromLogContext()
        .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

partial class Program
{
    public static string AppName => "Innermost.ReactApp.Aggregator";
    public static IConfiguration GetConfiguration()
    {
        var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();

        var config = builder.Build();

        return config;
    }
}

internal static class IServiceCollectionExtensions
{
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var identityServerUrl = configuration["IdentityServerUrl"];

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = identityServerUrl;
                options.RequireHttpsMetadata = configuration.GetValue<bool>("UseHttpsRedirection");
                options.Audience = "reactapigateway";

                if (configuration.GetValue<string>("LocalhostValidIssuer") != null)
                    options.TokenValidationParameters.ValidIssuers = new[] { configuration.GetValue<string>("LocalhostValidIssuer") };
            });

        return services;
    }

    public static IServiceCollection AddReactAppHttpAggregatorConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<UrlsConfig>(configuration.GetSection("urls"));

        return services;
    }

    public static IServiceCollection AddReactAppHttpAggregatorHttpClients(this IServiceCollection services)
    {
        services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddHttpClient<IDailyService, DailyService>();

        services
            .AddHttpClient<ILogLifeApiHttpClient, LogLifeApiHttpClient>()
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

        return services;
    }

    public static IServiceCollection AddReactAppHttpAggregatorGrpcServices(this IServiceCollection services)
    {
        services.AddScoped<ILifeRecordIntelligenceService, LifeRecordIntelligenceService>();

        return services;
    }

    public static IServiceCollection AddReactAppHttpAggregatorGrpcClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClient<IdentityUserStatueGrpc.IdentityUserStatueGrpcClient>(options =>
        {
            options.Address = new Uri(configuration.GetValue<string>("IdentityGrpcAddress"));
        });

        services.AddGrpcClient<IntelligenceLifeRecordRecommendationGrpc.IntelligenceLifeRecordRecommendationGrpcClient>(options =>
        {
            options.Address = new Uri(configuration.GetValue<string>("IntelligenceGrpcAddress"));
        });

        return services;
    }

    public static IServiceCollection AddCustomCORS(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("ReactApp", policy =>
            {
                policy
                    .WithOrigins("http://localhost:3000")
                    .SetIsOriginAllowed(_ => true)
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyMethod();
            });
        });

        return services;
    }
}