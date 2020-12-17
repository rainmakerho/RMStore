using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Logs;

using RMStore.Infrastructure;
using RMStore.Infrastructure.Filters;
using System.Diagnostics;

namespace RMStore.WebUI
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(cookieOptions => {
                    cookieOptions.LoginPath = "/";
                });
            services.AddRazorPages()
                .AddRazorPagesOptions(options => {
                    options.Conventions.AuthorizePage("/Product");
                })
                .AddMvcOptions(options =>
                {
                    //加入 Filter 來記錄 Razor Page 執行的起迄時間
                    options.Filters.Add(typeof(TrackPagePerformanceFilter));
                });
            services.AddHttpClient();

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
             
            

 
           //services.Add(new ServiceDescriptor(typeof(ActivitySource), new ActivitySource(serviceName)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //因為會寫 Log 所以不用 UseDeveloperExceptionPage
            app.UseExceptionHandler("/Error");
            
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            
        }
    }
}
