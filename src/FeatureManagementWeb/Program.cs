using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FeatureManagementWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                        .UseAzureAppConfiguration(
                        "WebApp:AppOptions",
                        "WebApp:AppOptions:BackgroundColor",
                        (connect, config) =>
                        {
                            config.Bind("AppConfig", connect);
                        },
                        (interval, config) =>
                        {
                            interval.RefreshInterval = null; //config.GetValue<TimeSpan>("AppConfig:RefreshInterval");
                        })
                        .ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.UseStartup<Startup>();
                        });
        }
    }
}
