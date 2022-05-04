using Autofac.Extensions.DependencyInjection;
using EventBusCommon.Abstractions;
using EventBusServiceBus;
using EventBusServiceBus.Extensions;
using Innermost.IServiceCollectionExtensions;
using Innermost.Meet.Infrastructure.Repositories;
using Innermost.Meet.SignalRHub.Application.IntegrationEventHandlers;
using Innermost.Meet.SignalRHub.Hubs;
using Innermost.Meet.SignalRHub.Infrastructure.AutofacModules;
using Innermost.MongoDBContext.Extensions.Microsoft.DependencyInjection;
using IntegrationEventServiceMongoDB.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

IConfiguration configuration = GetConfiguration();
Log.Logger = CreateSerilogLogger(configuration);

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory(config =>
    {
        config.RegisterModule<MediatRModule>();
        config.RegisterModule<IntegrationEventModule>();
    }))
    .ConfigureAppConfiguration(c => c.AddConfiguration(configuration))
    .UseContentRoot(Directory.GetCurrentDirectory())
    .UseSerilog();

// Add services to the container.

builder.Services.AddSignalR();

builder.Services
    .AddCustomAuthentication(configuration)
    .AddMeetMongoDBContext(configuration)
    .AddMeetRedisContext(configuration)
    .AddMeetSignalRHubRepositories()
    .AddMeetSignalRHubQueries()
    .AddMeetSignalRHubServices()
    .AddDefaultAzureServiceBusEventBus(configuration)
    .AddIntegrationEventServiceMongoDB()
    .AddMeetSignalRHubGrpcClients()
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
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chat", options =>
{
    options.CloseOnAuthenticationExpiration = true;
});

ConfigureMeetSignalRHubEventBus(app);

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

void ConfigureMeetSignalRHubEventBus(IApplicationBuilder app)
{
    var eventBus = app.ApplicationServices.GetRequiredService<IAsyncEventBus>();

    eventBus.Subscribe<AdminSendMessageToUserIntegrationEvent, AdminSendMessageToUserIntegrationEventHandler>();
}

partial class Program
{
    public static string AppName => "Innermost.Meet.SignalRHub";
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
                options.Audience = "meet";
            });

        return services;
    }

    public static IServiceCollection AddMeetMongoDBContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongoDBContext<MeetMongoDBContext>(config =>
        {
            config.WithConnectionString(configuration.GetConnectionString("MongoDB"));
            config.WithDatabase("InnermostMeet");
        });

        services.AddMongoDBSession();

        return services;
    }

    public static IServiceCollection AddMeetRedisContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<MeetRedisContext>(new MeetRedisContext(configuration.GetConnectionString("Redis")));

        return services;
    }

    public static IServiceCollection AddMeetSignalRHubRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserChattingContextRepository, UserChattingContextRepository>();

        return services;
    }

    public static IServiceCollection AddMeetSignalRHubQueries(this IServiceCollection services)
    {
        services.AddScoped<IUserChattingContextQueries, UserChattingContextQueries>();

        return services;
    }

    public static IServiceCollection AddMeetSignalRHubServices(this IServiceCollection services)
    {
        services.AddScoped<IChattingRecordRedisService, ChattingRecordRedisService>();

        return services;
    }
    public static IServiceCollection AddMeetSignalRHubGrpcClients(this IServiceCollection services)
    {
        services.AddGrpcClient<IdentityUserStatueGrpc.IdentityUserStatueGrpcClient>(options=>
        {
            options.Address = new Uri("https://localhost:5106");
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