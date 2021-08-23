using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CAF.Application.Dependency;
using CAF.UI.WebApi.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using CAF.Core.Helper;
using CAF.UI.WebApi.Utilities;
using DeryaBilisim.BiBayim.Integration.Standart;
using CAF.Core.Utilities;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Hangfire;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using CAF.Core.Modules.SignalR;
using CAF.Core.Modules.HttpPutObject;

namespace CAF.UI.WebApi
{
    //TODO: hotfix
    //TODO: hotfix2
    //TODO: hotfix3
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; private set; }

        public static IServiceProvider ServiceProvider { get; private set; }

        public static bool IsDevelopment
        {
            get
            {
                string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                return environment == "Development";
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (Environment.GetEnvironmentVariable("IS_DEBUGGER")?.ToUpper() == "TRUE")
            {
                System.Diagnostics.Debugger.Launch();
            }

            Configuration = GetBuildConfiguration();
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new HttpPutObjectConverter());
                }
            );
            services.AddMemoryCache();

            // Add detection services container and device resolver service.
            services.AddDetection();
            // Needed by Wangkanai Detection
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            #region Register DependencyInjection
            services.AddDbContextServices(Configuration);
            #endregion

            #region Swagger
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Clean Architecture API", Version = "v2" });
                c.OperationFilter<SwaggerFilter>();
                c.SchemaFilter<SwaggerEnumSchemaFilter>();

                // Set the comments path for the Swagger JSON and UI.
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                  {
                    new OpenApiSecurityScheme
                    {
                      Reference = new OpenApiReference
                        {
                          Type = ReferenceType.SecurityScheme,
                          Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                      },
                      new List<string>()
                    }
                  });
                //c.DocumentFilter<SwaggerAddEnumDescriptions>(); //Enum Dispaley Name


                //Api Xml
                var webApiAssembly = System.Reflection.Assembly.GetExecutingAssembly();
                var apiXmlStream = webApiAssembly.GetManifestResourceStream("CAF.UI.WebApi.CAF.UI.WebApi.xml");

                if (webApiAssembly != null && apiXmlStream != null)
                {
                    c.IncludeXmlComments(() =>
                    {
                        return new System.Xml.XPath.XPathDocument(apiXmlStream);
                    });
                }

                //Core Xml -
                var coreAssembly = typeof(Core.HttpPutObject<>).Assembly;
                var coreXmlStream = coreAssembly.GetManifestResourceStream("CAF.Core.CAF.Core.xml");
                if (coreAssembly != null && coreXmlStream != null)
                {
                    c.IncludeXmlComments(() =>
                    {
                        return new System.Xml.XPath.XPathDocument(coreXmlStream);
                    });
                }

            });
            #endregion

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            var connectionStringsSection = Configuration.GetSection("ConnectionStrings");

            services.AddHttpContextAccessor();

            #region Cryptographic
            services.AddDataProtection()
                    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
                    {
                        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_GCM,
                        ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                    });
            #endregion

            #region Authentication
            var secret = appSettingsSection.GetValue<string>("Secret");
            var key = Encoding.ASCII.GetBytes(secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            #endregion

            services.AddSingleton<Core.Interface.ISession, ApiSession>();
            services.AddScoped<ILogger, NLogLogger>();

            #region BiBayim
            var biBayimServiceSettingsSection = Configuration.GetSection("BiBayimIntegration");
            var biBayimServiceEndpoint = biBayimServiceSettingsSection.GetValue<string>("Endpoint");
            var biBayimServiceToken = biBayimServiceSettingsSection.GetValue<string>("Token");
            services.AddBiBayimService(biBayimServiceEndpoint, biBayimServiceToken);
            #endregion

            #region Cors
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                   builder.AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials()
                          .SetIsOriginAllowed((host) => true) //for signalr cors
                   );
            });
            #endregion

            #region Request Log & Other Filters
            var appSettings = ((IAppSettings)services.BuildServiceProvider().GetService(typeof(IAppSettings)));
            var useRequestLog = appSettings?.UseRequestLog;

            //services.AddResponseCaching();
            services.AddMvc(options =>
            {
                if (useRequestLog == true)
                    options.Filters.Add(new RequestLogFilter());

                options.Filters.Add(new ErrorFilter());
                options.Filters.Add(new ValidationFilter());
                options.Filters.Add(new AccessTokenFilter());


                options.Filters.Add(new ProducesResponseTypeAttribute((int)System.Net.HttpStatusCode.OK));
                options.Filters.Add(new ProducesResponseTypeAttribute(typeof(Core.Exception.ExceptionModel), (int)System.Net.HttpStatusCode.BadRequest));
                options.Filters.Add(new ProducesResponseTypeAttribute(typeof(Core.Exception.ExceptionModel), (int)System.Net.HttpStatusCode.NotFound));
                options.Filters.Add(new ProducesResponseTypeAttribute(typeof(Core.Exception.ExceptionModel), (int)System.Net.HttpStatusCode.InternalServerError));
            }).AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            #endregion

            #region Hangfire
            var dbConnection = connectionStringsSection.GetValue<string>("DbConnection");
            services.AddHangfire(x => x.UseSqlServerStorage(dbConnection));
            services.AddHangfireServer();
            #endregion

            #region SignalR
            services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                //hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1); // ui için de düzeltme gerekir disconnect olmaması için
            })
            .AddHubOptions<NotificationHub>(options =>
            {
                options.EnableDetailedErrors = true;
            });
            #endregion

            services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);

            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            Startup.ServiceProvider = services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            app.Use((context, next) =>
            {
                context.Request.EnableBuffering();
                return next();
            });
            //app.UseResponseCaching();
            app.UseCors();
            #region Pasif bırakılan cars ayarları
            //app.UseCors("CorsPolicy");
            //app.UseCors("CorsPolicy");
            //app.UseCors("AllowAllCors"); 
            #endregion
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            if (!env.IsProduction())
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Derya Sigorta API V1");
                    //c.RoutePrefix = string.Empty;
                });
            }
            //wangkanai.Detection
            app.UseDetection();

            app.UseCookiePolicy();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            #region Hangfire
            // The rest of the hangfire config as usual.
            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            // Configure hangfire to use the new JobActivator we defined.
            GlobalConfiguration.Configuration
                .UseActivator(new HangfireActivator(serviceProvider));
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
                #region SignalR
                endpoints.MapHub<NotificationHub>("/hubs/notificationHub");
                #endregion
            });

            var cultureInfo = new CultureInfo("tr-TR");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            Core.Utilities.AppStatic.Session = new ApiSession(Startup.ServiceProvider.GetService<IHttpContextAccessor>());

            OnApplicationStarted(Startup.ServiceProvider.GetService<Core.Service.ISettingService>());
        }

        internal static IConfiguration GetBuildConfiguration()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables();
            return builder.Build();
        }

        public void OnApplicationStarted(Core.Service.ISettingService settingService)
        {
            //Servis başladığında oluşturulmamış günlük jubların oluşturulması
            settingService.HangFireJonRegister();
        }
    }

}

