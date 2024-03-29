﻿using EventBusServiceBus.Extensions;
using Innermost.Identity.API.Grpc.Services;
using IntegrationEventServiceSQL.Extensions;

namespace Innermost.Identity.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var sqlConnectionString = Configuration.GetConnectionString("ConnectMySQL");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            //EF Core MySQL 连接配置
            services.AddDbContext<InnermostIdentityDbContext>(options =>
                options.UseMySql(
                    sqlConnectionString,
                    new MySqlServerVersion(new Version(8, 0)),
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(migrationsAssembly);
                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    }
                )
            );

            //redis context
            services.AddSingleton<UserStatueRedisContext>(new UserStatueRedisContext(Configuration.GetConnectionString("Redis")));

            //数据库健康检查
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddMySql(sqlConnectionString,
                    name: "IdentityDB-Check",
                    tags: new string[] { "IdentityDB" });

            //添加transient依赖
            services.AddTransient<ILoginService<InnermostUser>, InnermostLoginService>();

            //Add scoped dependencies
            services.AddScoped<IUserStatueService, UserStatueService>();

            //添加 ASP.NET Identity
            services.AddIdentity<InnermostUser, IdentityRole>()
                .AddEntityFrameworkStores<InnermostIdentityDbContext>()
                .AddDefaultTokenProviders();

            //Add cors
            services.AddCors(options =>
            {
                options.AddPolicy("WebApp", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:3000")
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .AllowAnyMethod();
                });
            });

            //添加 IdentityServer
            var builder = services.AddIdentityServer(options =>
            {
                options.Authentication.CookieLifetime = TimeSpan.FromHours(2);
                //TODO here can also configure cors.
            })
            .AddAspNetIdentity<InnermostUser>()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder => builder.UseMySql(sqlConnectionString,
                    new MySqlServerVersion(new Version(8, 0)),
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(migrationsAssembly);
                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder => builder.UseMySql(sqlConnectionString,
                    new MySqlServerVersion(new Version(8, 0)),
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(migrationsAssembly);
                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            });

            //Add Authorization Policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
                options.AddPolicy("User", policy => policy.RequireClaim(ClaimTypes.Role, "User"));
                options.AddPolicy("IntenalService", policy => policy.RequireClaim("client_id", "serviceclient"));
            });

            //对账号密码等信息配置
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
                options.User.RequireUniqueEmail = true;
            });

            //configure lifetime of tokens for confirm
            builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>o.TokenLifespan = TimeSpan.FromMinutes(10));

            //配置cookie
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie.Name = "InnermostUserCookie";
                options.LoginPath = "/Account/Login";
            });

            //Gprc
            services.AddGrpc();

            //add event bus and integration event service
            services
                .AddDefaultAzureServiceBusEventBus(Configuration)
                .AddIntegrationEventServiceSQL<InnermostIdentityDbContext>();

            //开发使用的证书，真正生产环境下需要像eshop项目里一样弄一个证书装进去
            builder.AddDeveloperSigningCredential();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Innermost.Identity.API", Version = "v1" });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Innermost.Identity.API v1"));
            }
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Content-Security-Policy", "script-src 'unsafe-inline'");
                await next();
            });
            app.UseCors("WebApp");
            app.UseIdentityServer();

            if(Configuration.GetValue<bool>("UseHttpsRedirection"))
                app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<IdentityUserGrpcService>();
                endpoints.MapGrpcService<IdentityUserStatueGrpcService>();
            });
        }
    }
}
