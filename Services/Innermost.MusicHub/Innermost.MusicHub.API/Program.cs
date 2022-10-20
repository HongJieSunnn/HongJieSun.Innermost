using Autofac;
using Autofac.Extensions.DependencyInjection;
using Innermost.MusicHub.API.Grpc.Services;
using Innermost.MusicHub.API.Infrastructure.AutofacModules;
using Innermost.MusicHub.API.Queries.AlbumQueries;
using Innermost.MusicHub.API.Queries.MusicRecordQueries;
using Innermost.MusicHub.API.Queries.SingerQueries;
using Innermost.MusicHub.Domain.Repositories;
using Innermost.MusicHub.Infrastructure.Repositories;
using IntegrationEventServiceMongoDB.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

IConfiguration configuration = GetConfiguration();
Log.Logger = CreateSerilogLogger(configuration);


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory(config =>
    {
        config.RegisterModule<MediatRModule>();
    }))
    .ConfigureAppConfiguration(c => c.AddConfiguration(configuration))
    .UseContentRoot(Directory.GetCurrentDirectory())
    .UseSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
        .AddCustomAuthentication(configuration)
        .AddMusicHubMongoDBContext(configuration)
        .AddIntegrationEventServiceMongoDB()
        .AddMusicHubRepositories()
        .AddMusicHubQueries()
        .AddMusicHubgRPC()
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
app.MapGrpcService<MusicRecordGrpcService>();

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
    public static string AppName => "Innermost.MusicHub.API";
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
                options.Audience = "musichub";

                if (configuration.GetValue<string>("LocalhostValidIssuer") != null)
                    options.TokenValidationParameters.ValidIssuers = new[] { configuration.GetValue<string>("LocalhostValidIssuer") };
            });

        return services;
    }

    public static IServiceCollection AddMusicHubMongoDBContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongoDBContext<MusicHubMongoDBContext>(config =>
        {
            config.WithConnectionString(configuration.GetConnectionString("MongoDB"));
            config.WithDatabase("InnermostMusicHub");
        });

        //services.AddMongoDBSession();

        return services;
    }

    public static IServiceCollection AddMusicHubRepositories(this IServiceCollection services)
    {
        services.AddScoped<IMusicRecordRepository, MusicRecordRepository>();
        services.AddScoped<IAlbumRepository, AlbumRepository>();
        services.AddScoped<ISingerRepository, SingerRepository>();

        return services;
    }

    public static IServiceCollection AddMusicHubQueries(this IServiceCollection services)
    {
        services.AddScoped<IMusicRecordQueries, MusicRecordQueries>();
        services.AddScoped<IAlbumQueries, AlbumQueries>();
        services.AddScoped<ISingerQueries, SingerQueries>();

        return services;
    }

    public static IServiceCollection AddMusicHubgRPC(this IServiceCollection services)
    {
        services.AddGrpc();
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