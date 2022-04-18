using EventBusCommon;
using EventBusCommon.Abstractions;
using EventBusServiceBus;
using Innermost.IdempotentCommand.Extensions.Microsoft.DependencyInjection;
using Innermost.TagReferrers;
using Innermost.TagServer.API.Infrastructure.AutofacModules;
using IServiceCollectionExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Innermost.TagServer.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services
                .AddControllers();

            services.AddTagSServer(config =>
            {
                config.WithConnectionString(Configuration.GetConnectionString("MongoDB"));
                config.WithDatabase("InnermostTagServer");
            });

            services.AddIdempotentCommandRequestStorage();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["IdentityServerUrl"];
                    options.Audience = "tagserver";
                });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IIdentityService, IdentityService>();

            services.AddMongoDBSession();

            services.AddEventBus(Configuration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Innemost.TagServer.API", Version = "v1" });
                c.CustomSchemaIds(t => t.FullName);
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri($"{Configuration["IdentityServerUrl"]}/connect/authorize"),
                            TokenUrl = new Uri($"{Configuration["IdentityServerUrl"]}/connect/token"),
                            Scopes = new Dictionary<string, string>()
                            {
                                { "tagserver", "TagServer API" }
                            }
                        }
                    }
                });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Innemost.TagServer.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });

            ConfigureTagSServer(app);
            app.ConfigureTagServerEventBus();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<MediatRModule>();
        }

        private void ConfigureTagSServer(IApplicationBuilder builder)
        {
            builder.MapTagSMongoDBCollectionModels();
            builder.AddReferrerDiscriminator<LifeRecordReferrer>();
            builder.AddLocationIndexFroReferrer("BaiduPOI");
            builder.SeedDefaultEmotionTags();
            builder.SeedDefaultMusicTags();
        }
    }

    internal static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var subcriptionName = configuration["SubscriptionClientName"];

            services.AddSingleton<IServiceBusPersisterConnection>(sp =>
            {
                var connectionString = configuration.GetSection("EventBusConnections")["ConnectAzureServiceBus"];
                var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersisterConnection>>();

                return new DefaultServiceBusPersisterConnection(connectionString, logger);
            });

            services.AddSingleton<IAsyncEventBus, EventBusAzureServiceBus>(sp =>
            {
                var persister = sp.GetRequiredService<IServiceBusPersisterConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusAzureServiceBus>>();
                var subcriptionManager = sp.GetRequiredService<IEventBusSubscriptionManager>();
                var lifescope = sp.GetRequiredService<ILifetimeScope>();

                return new EventBusAzureServiceBus(persister, logger, subcriptionManager, subcriptionName, lifescope, subcriptionName);
            });

            services.AddSingleton<IEventBusSubscriptionManager, InMemoryEventBusSubscriptionsManager>();

            return services;
        }
    }
}
