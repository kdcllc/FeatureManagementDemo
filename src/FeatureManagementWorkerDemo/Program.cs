using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;

namespace FeatureManagementWorkerDemo
{
    public sealed class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args)
                    .UseConsoleLifetime(options => options.SuppressStatusMessages = true)
                    .Build();

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                        .UseAzureAppConfiguration(
                        "WorkerApp:WorkerOptions",
                        "WorkerApp:WorkerOptions:Message",
                        options =>
                        {
                            options.UseFeatureFlags(flags =>
                            {
                                flags.CacheExpirationTime = TimeSpan.FromSeconds(1);
                            });
                        })
                        .ConfigureServices((hostContext, services) =>
                        {
                            services.AddFeatureManagement()
                                    .AddDefaultFeatureManagement();

                            services.AddOptionsWithChangeToken<WorkerOptions>(
                                "WorkerApp:WorkerOptions",
                                configureAction: (o) => { });

                            services.AddHostedService<Worker>();
                        });
        }
    }
}
