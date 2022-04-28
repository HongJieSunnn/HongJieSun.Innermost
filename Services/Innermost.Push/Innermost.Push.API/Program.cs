using Autofac.Extensions.DependencyInjection;
using EventBusServiceBus;
using EventBusServiceBus.Extensions;
using Innermost.Push.API.Application.IntegrationEventHandlers;
using Innermost.Push.API.Infrastructure.AutofacModules;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;


IConfiguration configuration = GetConfiguration();
Log.Logger = CreateSerilogLogger(configuration);

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory(config =>
    {
        config.RegisterModule<IntegrationEventModule>();
    }))
    .ConfigureAppConfiguration(c => c.AddConfiguration(configuration))
    .UseContentRoot(Directory.GetCurrentDirectory())
    .UseSerilog()
    .Build();

// Add services to the container.

builder.Services.AddSignalR();

builder.Services
    .AddCustomAuthentication(configuration)
    .AddDefaultAzureServiceBusEventBus(configuration)
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
app.MapHub<PushHub>("/push");

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

    eventBus.Subscribe<PushMessageToUserIntegrationEvent, PushMessageToUserIntegrationEventHandler>();
    eventBus.Subscribe<SendMailIntegrationEvent, SendMailIntegrationEventHandler>();
}

partial class Program
{
    public static string AppName => "Innermost.Push.API";
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
                options.Audience = "push";
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