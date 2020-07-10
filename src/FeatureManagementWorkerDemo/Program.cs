using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace FeatureManagementWorkerDemo
{
    public class Program
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
                        (connect, config) =>
                        {
                            config.Bind("AppConfig", connect);
                        },
                        (interval, config) =>
                        {
                            interval.RefreshInterval = config.GetValue<TimeSpan>("AppConfig:RefreshInterval");
                        })
                        .ConfigureServices((hostContext, services) =>
                        {
                            services.AddOptions<WorkerOptions>()
                                    .Configure<IConfiguration>((op, config) =>
                                    {
                                        op.Message = config.GetValue<string>("Worker:Message");
                                    });

                            services.AddHostedService<Worker>();
                        });
        }
    }
}
