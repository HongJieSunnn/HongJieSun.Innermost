using EventBusServiceBus.Extensions;
using Innermost.IdempotentCommand.Extensions.Microsoft.DependencyInjection;
using Innermost.Identity.API.UserStatue;
using Innermost.Meet.API.Application.IntegrationEventHandles;
using Innermost.Meet.API.Infrastructure.AutofacModules;
using Innermost.Meet.Infrastructure.Repositories;
using Innermost.MongoDBContext.Extensions.Microsoft.DependencyInjection;
using IntegrationEventServiceMongoDB.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Innermost.Meet.API
{
    internal class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services
                .AddMeetAuthentication(Configuration)
                .AddMeetMongoDBContext(Configuration)
                .AddMeetRepositories()
                .AddMeetQueries()
                .AddDefaultAzureServiceBusEventBus(Configuration)
                .AddIntegrationEventServiceMongoDB()
                .AddIdempotentCommandRequestStorage()
                .AddUserIdentityService(Configuration)
                .AddMeetGrpcClients(Configuration)
                .AddCustomCORS();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Innemost.Meet.API", Version = "v1" });
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
                                { "meet", "Meet API" }
                            }
                        }
                    }
                });
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("ReactApp");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Innemost.TagServer.API v1"));
            }

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

            ConfigureMeetEventBus(app);
        }

        public void ConfigureMeetEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IAsyncEventBus>();

            eventBus.Subscribe<LifeRecordSetSharedIntegrationEvent, LifeRecordSetSharedIntegrationEventHandler>();
            eventBus.Subscribe<UserRegisteredIntegrationEvent, UserRegisteredIntegrationEventHandler>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<MediatRModule>();
            builder.RegisterModule<IntegrationEventModule>();
        }
    }

    internal static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMeetAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["IdentityServerUrl"];
                    options.RequireHttpsMetadata = configuration.GetValue<bool>("UseHttpsRedirection");
                    options.Audience = "meet";

                    if (configuration.GetValue<string>("LocalhostValidIssuer") != null)
                        options.TokenValidationParameters.ValidIssuers = new[] { configuration.GetValue<string>("LocalhostValidIssuer") };
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

        public static IServiceCollection AddMeetRepositories(this IServiceCollection services)
        {
            services.AddScoped<ISharedLifeRecordRepository, SharedLifeRecordRepository>();
            services.AddScoped<IUserChattingContextRepository, UserChattingContextRepository>();
            services.AddScoped<IUserInteractionRepository, UserInteractionRepository>();
            services.AddScoped<IUserSocialContactRepository, UserSocialContactRepository>();

            return services;
        }

        public static IServiceCollection AddMeetQueries(this IServiceCollection services)
        {
            services.AddScoped<IOwnSharedLifeRecordQueries, OwnSharedLifeRecordQueries>();
            services.AddScoped<IMeetSharedLifeRecordQueries, MeetSharedLifeRecordQueries>();
            services.AddScoped<IInteractionQueries, InteractionQueries>();
            services.AddScoped<ISocialContactQueries, SocialContactQueries>();
            services.AddScoped<IStatueQueries, StatueQueries>();

            return services;
        }

        public static IServiceCollection AddIdempotentCommondRequestStorage(this IServiceCollection services)
        {
            services.AddIdempotentCommandRequestStorage();

            return services;
        }

        public static IServiceCollection AddUserIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityProfileService(configuration.GetValue<string>("IdentityGrpcAddress"));

            return services;
        }

        public static IServiceCollection AddMeetGrpcClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpcClient<IdentityUserStatueGrpc.IdentityUserStatueGrpcClient>(options =>
            {
                options.Address = new Uri(configuration.GetValue<string>("IdentityGrpcAddress"));
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
}
