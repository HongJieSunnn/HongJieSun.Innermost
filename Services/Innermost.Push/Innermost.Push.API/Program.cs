using Autofac.Extensions.DependencyInjection;
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
    .UseSerilog();

// Add services to the container.

builder.Services.AddSignalR();

builder.Services
    .AddCustomAuthentication(configuration)
    .AddDefaultAzureServiceBusEventBus(configuration)
    .AddEmailService(configuration)
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
                options.Audience = "push";

                if (configuration.GetValue<string>("LocalhostValidIssuer") != null)
                    options.TokenValidationParameters.ValidIssuers = new[] { configuration.GetValue<string>("LocalhostValidIssuer") };
            });

        return services;
    }

    public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        var mailGunSection = configuration.GetSection("MailGun");
        var fromEmail = mailGunSection.GetValue<string>("FromEmail");
        var fromName = mailGunSection.GetValue<string>("FromName");
        var domainName = mailGunSection.GetValue<string>("DomainName");
        var apiKey = mailGunSection.GetValue<string>("ApiKey");
        services
            .AddFluentEmail(fromEmail, fromName)
            .AddMailGunSender(domainName, apiKey);

        services.AddTransient<ISendEmailService, SendEmailService>();

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