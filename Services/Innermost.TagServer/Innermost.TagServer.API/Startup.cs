using EventBusServiceBus.Extensions;
using Innermost.IdempotentCommand.Extensions.Microsoft.DependencyInjection;
using Innermost.TagReferrers;
using Innermost.TagServer.API.Infrastructure.AutofacModules;
using IntegrationEventServiceMongoDB.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Security.Claims;
using TagS.Microservices.Server.Microsoft.AspNetCore.Http;
using TagS.Microservices.Server.Microsoft.DependencyInjection;
using TagS.Microservices.Server.Models;

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
                    options.RequireHttpsMetadata = Configuration.GetValue<bool>("UseHttpsRedirection");
                    options.Audience = "tagserver";

                    if (Configuration.GetValue<string>("LocalhostValidIssuer") != null)
                        options.TokenValidationParameters.ValidIssuers = new[] { Configuration.GetValue<string>("LocalhostValidIssuer") };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
            });

            services.AddIdentityService();

            services.AddMongoDBSession();

            services
                .AddDefaultAzureServiceBusEventBus(Configuration)
                .AddIntegrationEventServiceMongoDB();

            services.AddCustomCORS();

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
            app.UseCors("ReactApp");
            if (Configuration.GetValue<bool>("UseHttpsRedirection"))
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
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<MediatRModule>();
        }

        private void ConfigureTagSServer(IApplicationBuilder builder)
        {
            builder.MapTagSMongoDBCollectionModels();
            builder.SeedDefaultEmotionTags();
            builder.SeedDefaultMusicTags();
            builder.ConfigureTagServerEventBus();

            builder.AddReferrerDiscriminator<LifeRecordReferrer>();
            builder.AddReferrerIndexes<LifeRecordReferrer>("UserId", "IsShared", "LocationUId", "MusicRecordMId", "CreateTime");
            builder.AddReferrerIndexes<LifeRecordReferrer>(Builders<TagWithReferrer>.IndexKeys.Geo2DSphere("Referrers.BaiduPOI"));
        }
    }

    internal static class IServiceCollectionExtensions
    {
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
}
