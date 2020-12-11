using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RMStore.Domain;
using Serilog;

namespace RMStore.API
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        public  static void Main(string[] args)
        {
            //OpenTelemetry .NET 5 before versions
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
            try
            {
                Log.Information(messageTemplate: "Start API Application");
                var host = CreateHostBuilder(args).Build();
                //application init ...
                using (var scope = host.Services.CreateScope())
                {
                    var serviceProvider = scope.ServiceProvider;
                    var hostingEnvironment = serviceProvider
                        .GetRequiredService<IHostEnvironment>();
                    var ctx = serviceProvider.GetRequiredService<InMemoryDbContext>();
                    ctx.Database.EnsureCreated();
                }
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, messageTemplate: "API App GG!");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog();
                
    }
}
