using Innermost.IdempotentCommand.Extensions.Microsoft.DependencyInjection;
using TagS.Microservices.Client.Microsoft.DependencyInjection;

namespace Innermost.LogLife.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services
                .AddControllers(options =>
                {
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                })
                .Services
                .AddHealthCheck(Configuration)
                .AddCustomDbContext(Configuration)
                .AddCustomEventBus(Configuration)
                .AddCustomIntegrationEventConfiguration(Configuration)
                .AddCustomAuthentication(Configuration)
                .AddGrpcServices(Configuration)
                .AddCustomAutoMapper(Configuration)
                .AddQueriesAndRepositories(Configuration)
                .AddCustomConfig(Configuration)
                .AddTagSClient()
                .AddIdempotentCommandRequestSQLStorage<LifeRecordDbContext>();
    

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Innemost.LogLife.API", Version = "v1" });
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
                                { "loglife", "LogLife API" }
                            }
                        }
                    }
                });
            });

            var container = new ContainerBuilder();

            container.Populate(services);
            container.RegisterModule<MediatRModules>();

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Innemost.LogLife.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            ConfigureAuth(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/health/database", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
                {
                    Predicate = r => r.Name == "loglife"
                });
            });

            ConfigureEventBus(app);
        }

        private void ConfigureAuth(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            //var eventBus = app.ApplicationServices.GetRequiredService<IAsyncEventBus>();

            //TODO Register the Handlers.
        }
    }

    static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            var healthCheckBuilder = services.AddHealthChecks();

            healthCheckBuilder.AddCheck("loglife", () => HealthCheckResult.Healthy());//对该服务进行检查，第二个参数实际上是检查的具体逻辑，由于能检查到那么一定健康，所以返回Healthy就好。

            healthCheckBuilder
                .AddMySql(
                    configuration["ConnectMySQL"],
                    "Innermost.LogLifeMySQLDB-Check",
                    tags: new string[] { "loglifemysqldb" });

            healthCheckBuilder
                .AddAzureServiceBusTopic(
                    configuration["ConnectAzureServiceBus"],
                    "innermost_event_bus",
                    "Innermost.LogLife-AzureServiceBus-Check",
                    tags: new string[] { "loglifeservicebus" });

            return services;
        }

        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContext<LifeRecordDbContext>(options =>
                {
                    options
                        .UseMySql(configuration.GetConnectionString("ConnectMySQL"), new MySqlServerVersion(new Version(5, 7)), options =>
                        {
                            options.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);

                            options.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        });
                });

            services
                .AddDbContext<IntegrationEventRecordDbContext>(options =>
                {
                    options
                        .UseMySql(configuration.GetConnectionString("ConnectMySQL"), new MySqlServerVersion(new Version(5, 7)), options =>
                        {
                            options.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);

                            options.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        });
                });

            //TODO redis/mongodb/log

            return services;
        }

        public static IServiceCollection AddCustomIntegrationEventConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IntegrationEventRecordServiceFactory>();
            services.AddTransient<ILogLifeIntegrationEventService, LogLifeIntegrationEventService>();
            services.AddTransient<IIntegrationEventService, LogLifeIntegrationEventService>();//TODO maybe we should not inject ILogLifeIntegrationEventService.
            
            services.AddSingleton<IServiceBusPersisterConnection>(sp =>
            {
                var connectionString = configuration.GetSection("EventBusConnections")["ConnectAzureServiceBus"];
                var logger = sp.GetService<ILogger<DefaultServiceBusPersisterConnection>>();

                return new DefaultServiceBusPersisterConnection(connectionString, logger);
            });

            return services;
        }

        public static IServiceCollection AddCustomEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var subcriptionName = configuration["SubscriptionClientName"];

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

        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");//Remove防止被过滤,从而可以出现在HttpContext.User Claims中

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
                    options.Audience = "loglife";
                });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IIdentityService, IdentityService>();

            return services;
        }

        public static IServiceCollection AddGrpcServices(this IServiceCollection service, IConfiguration configuration)
        {
            

            return service;
        }

        public static IServiceCollection AddCustomAutoMapper(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper((sp, options) =>
            {
                //var identityService = sp.GetService<IIdentityService>();

                //options.AddMaps(new Type[] { typeof(MusicDetailDTO), typeof(MusicDetail) ,typeof(CreateOneRecordCommand),typeof(UpdateOneRecordCommand),typeof(LifeRecord) });

                
            }, Assembly.GetExecutingAssembly());

            return services;
        }

        public static IServiceCollection AddQueriesAndRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnectMySQL");

            services.AddScoped<ILifeRecordQueries, LifeRecordQueries>(sp =>
            {
                var identityServer = sp.GetRequiredService<IIdentityService>();
                return new LifeRecordQueries(connectionString, identityServer);
            });

            services.AddScoped<ILifeRecordRepository, LifeRecordRepository>();

            //TODO AddIdempotentCommandRequestStorage

            return services;
        }

        /// <summary>
        /// 使Model验证错误时返回的信息更详细。
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var modelInvalidDetail = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Instance = context.HttpContext.Request.Path,
                        Detail = "Please check error by error respone"
                    };

                    return new BadRequestObjectResult(modelInvalidDetail);
                };
            });

            return services;
        }
    }
}
