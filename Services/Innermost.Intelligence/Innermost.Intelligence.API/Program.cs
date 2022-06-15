using Autofac.Extensions.DependencyInjection;
using Azure;
using EventBusCommon.Abstractions;
using Innermost.Intelligence.API.Grpc.Services;
using Innermost.Intelligence.API.Services.DailySentence;
using Innermost.Intelligence.API.Services.Recommendations;
using Innermost.Intelligence.API.Services.TextAnalytics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Net;

IConfiguration configuration = GetConfiguration();
Log.Logger = CreateSerilogLogger(configuration);

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureAppConfiguration(c => c.AddConfiguration(configuration))
    .UseContentRoot(Directory.GetCurrentDirectory())
    .UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddCustomAuthentication(configuration)
    .AddHttpClientServices()
    .AddTextAnalyticsServices(configuration)
    .AddIntelligenceAPIServices()
    .AddIntelligenceGrpcServices()
    .AddIntelligenceGrpcClients(configuration)
    .AddCustomCORS();

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
app.MapGrpcService<IntelligenceLifeRecordRecommendationService>();

app.Run();

Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
{
    return new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .Enrich.WithProperty("ApplicationContext", AppName)
        .Enrich.FromLogContext()
        .WriteTo.Console(theme: AnsiConsoleTheme.Code)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

partial class Program
{
    public static string AppName => "Innermost.Intelligence.API";
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
                options.Audience = "intelligence";

                if (configuration.GetValue<string>("LocalhostValidIssuer") != null)
                    options.TokenValidationParameters.ValidIssuers = new[] { configuration.GetValue<string>("LocalhostValidIssuer") };
            });

        return services;
    }

    public static IServiceCollection AddHttpClientServices(this IServiceCollection services)
    {
        services.AddHttpClient();

        return services;
    }

    public static IServiceCollection AddTextAnalyticsServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<TextAnalyticsClient>((sp) =>
        {
            var endpoint = new Uri(configuration.GetSection("TextAnalytics").GetValue<string>("Endpoint"));
            var apiKey = new AzureKeyCredential(configuration.GetSection("TextAnalytics").GetValue<string>("ApiKey"));
            return new TextAnalyticsClient(endpoint, apiKey);
        });

        return services;
    }

    public static IServiceCollection AddIntelligenceAPIServices(this IServiceCollection services)
    {
        services.AddScoped<IDailySentenceService, DailySentenceService>();
        services.AddScoped<ILifeRecordRecommendationService, LifeRecordRecommendationService>();
        services.AddScoped<ITextAnalyticsService, TextAnalyticsService>();

        return services;
    }

    public static IServiceCollection AddIntelligenceGrpcServices(this IServiceCollection services)
    {
        services.AddGrpc();

        return services;
    }

    public static IServiceCollection AddIntelligenceGrpcClients(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddGrpcClient<MusicRecordGrpc.MusicRecordGrpcClient>(options =>
        {
            options.Address = new Uri(configuration.GetValue<string>("MusicHubGrpcAddress"));
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