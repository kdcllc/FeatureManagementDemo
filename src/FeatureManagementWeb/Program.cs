using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace FeatureManagementWeb
{
    public sealed class Program
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
                        _ => { },
                        (connect, config) =>
                        {
                            config.Bind("AppConfig", connect);
                        },
                        (interval, config) =>
                        {
                            // specifying null doesn't enable AppConfigurationWorker
                            interval.RefreshInterval = null;
                        })
                        .ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.UseStartup<Startup>();
                        });
        }
    }
}
