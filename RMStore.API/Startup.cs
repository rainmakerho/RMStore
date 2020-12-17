using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RMStore.Infrastructure.Middleware;
using RMStore.Domain;
using RMStore.Infrastructure.Filters;
using RMStore.Infrastructure;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry;
using System.Net.Http;

namespace RMStore.API
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
            services.AddSingleton<IScopeInformation, ScopeInformation>();

            var jwtSettings = Configuration.GetSection("JWT").Get<JwtSettings>();
            // Adding Authentication  
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })// Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.ValidAudience,
                    ValidIssuer = jwtSettings.ValidIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });

            services.AddDbContext<InMemoryDbContext>(options =>
                      options.UseInMemoryDatabase("rmstore"));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(TrackActionPerformanceFilter));
            });

            //OpenTelemetry
            var serviceName = typeof(Startup).Namespace;
            services.AddOpenTelemetryTracing((builder) => builder
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault().AddService(serviceName))
                .AddSource(serviceName)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddJaegerExporter(jaegerOptions =>
                {
                    jaegerOptions.AgentHost = "localhost";
                    jaegerOptions.AgentPort = 6831;
                })
                
                .AddConsoleExporter()
           );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //使用自定的 Exception Handler
            app.UseApiExceptionHandler(options => { 
                options.AddResponseDetails = UpdateApiErrorResponse;
                options.DetermineLogLevel = DetermineLogLevel;
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private LogLevel DetermineLogLevel(Exception ex)
        {
            if (ex.Message.StartsWith("cannot open database", StringComparison.InvariantCultureIgnoreCase) )
            {
                return LogLevel.Critical;
            }
            return LogLevel.Error;
        }

        private void UpdateApiErrorResponse(HttpContext context, Exception ex, ApiError error)
        {
            if(ex.GetType().Name == typeof(SqlException).Name)
            {
                error.Detail = "這是Database的錯誤";
            }
        }
    }
}
