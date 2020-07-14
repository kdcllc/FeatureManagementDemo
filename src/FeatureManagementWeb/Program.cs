using System;

using Microsoft.AspNetCore.Hosting;
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
                             "WebApp:AppOptions*",
                             "WebApp:AppOptions:Flag",
                             configureAzureAppConfigOptions: options =>
                             {
                                 options.UseFeatureFlags(flags =>
                                 {
                                     flags.CacheExpirationTime = TimeSpan.FromSeconds(1);
                                 });
                             })
                        .ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.UseStartup<Startup>();
                        });
        }
    }
}
