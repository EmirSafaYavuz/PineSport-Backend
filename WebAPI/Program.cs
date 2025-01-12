using System.Configuration;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.Helpers;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encyption;
using Core.Utilities.Security.Jwt;
using Core.Utilities.TaskScheduler.Hangfire.Models;
using Hangfire;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Globalization;
using System.Text.Json.Serialization;
using Business.DependencyResolvers.Autofac;
using Core.DependencyResolvers;
using DataAccess.Concrete.EntityFramework.Contexts;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
			
            var builder = WebApplication.CreateBuilder(args);

// Autofac ile DI kullanımı
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(container =>
            {
                // Autofac modüllerini burada ekleyebilirsiniz
                container.RegisterModule(new AutofacBusinessModule());
            });

// Logging yapılandırması
            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(LogLevel.Trace);

// Controller ve JSON yapılandırmaları
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

// API versiyonlama
            builder.Services.AddApiVersioning(v =>
            {
                v.DefaultApiVersion = new ApiVersion(1, 0);
                v.AssumeDefaultVersionWhenUnspecified = true;
                v.ReportApiVersions = true;
            });

// CORS yapılandırması
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

// Token doğrulama
            var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),
                        ClockSkew = TimeSpan.Zero
                    };
                });

// Swagger yapılandırması
            builder.Services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(Path.ChangeExtension(typeof(Program).Assembly.Location, ".xml"));
            });

// Logger ve diğer bağımlılıklar
            builder.Services.AddTransient<FileLogger>();
            builder.Services.AddTransient<PostgreSqlLogger>();
            builder.Services.AddTransient<MsSqlLogger>();
            builder.Services.AddScoped<IpControlAttribute>();

// Hangfire yapılandırması
            var taskSchedulerConfig = builder.Configuration.GetSection("TaskSchedulerOptions").Get<TaskSchedulerConfig>();
            if (taskSchedulerConfig.Enabled)
            {
                builder.Services.AddHangfire(config => config.UseSqlServerStorage(taskSchedulerConfig.ConnectionString));
                builder.Services.AddHangfireServer();
            }

            var coreModule = new CoreModule();
            builder.Services.AddDependencyResolvers(builder.Configuration, new ICoreModule[] { coreModule });
            builder.Services.AddDbContext<ProjectDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PgContext"))
                    .EnableSensitiveDataLogging());

            var app = builder.Build();

// ServiceTool kullanımı
            ServiceTool.ServiceProvider = app.Services;

// Ortam ayarları
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.ConfigureCustomExceptionMiddleware();

//app.UseDbOperationClaimCreator();

// Swagger
            if (!app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PineSport");
                    c.DocExpansion(DocExpansion.None);
                });
            }

// CORS
            app.UseCors("AllowOrigin");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

// Localization
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("tr-TR"),
            });

            var cultureInfo = new CultureInfo("tr-TR")
            {
                DateTimeFormat = { ShortTimePattern = "HH:mm" }
            };

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Hangfire dashboard
            if (taskSchedulerConfig.Enabled)
            {
                app.UseHangfireDashboard(taskSchedulerConfig.Path, new DashboardOptions
                {
                    DashboardTitle = taskSchedulerConfig.Title,
                    Authorization = new[]
                    {
                        new HangfireCustomBasicAuthenticationFilter
                        {
                            User = taskSchedulerConfig.Username,
                            Pass = taskSchedulerConfig.Password
                        }
                    }
                });
            }

            app.MapControllers();

            app.Run();
        }
    }
}