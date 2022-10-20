using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

IConfiguration configuration = GetConfiguration();
Log.Logger = CreateSerilogLogger(configuration);

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddCustomAuthentication(configuration)
    .AddCustomCORS()
    .AddTencentCloudCos(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("ReactApp");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
        .WriteTo.Console(theme: AnsiConsoleTheme.Code)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

partial class Program
{
    public static string AppName => "Innermost.FileServer.API";
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
                options.Audience = "fileserver";

                if (configuration.GetValue<string>("LocalhostValidIssuer") != null)
                    options.TokenValidationParameters.ValidIssuers = new[] { configuration.GetValue<string>("LocalhostValidIssuer") };
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

    public static IServiceCollection AddTencentCloudCos(this IServiceCollection services, IConfiguration configuration)
    {
        var tencentCloudCosSection = configuration.GetSection("TencentCloudCos");

        services.AddScoped<ITencentClouldCosSTSService, TencentClouldCosSTSService>(
            tc => new TencentClouldCosSTSService(
                tencentCloudCosSection["bucket"],
                tencentCloudCosSection["region"],
                tencentCloudCosSection.GetSection("allowPrefixes").Get<string[]>(),
                tencentCloudCosSection.GetSection("allowActions").Get<string[]>(),
                tencentCloudCosSection.GetValue<int>("durationSeconds"),
                tencentCloudCosSection["secretId"],
                tencentCloudCosSection["secretKey"]
                ));

        return services;
    }
}