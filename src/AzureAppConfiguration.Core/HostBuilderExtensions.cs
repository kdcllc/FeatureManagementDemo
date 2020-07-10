using Azure.Identity;
using AzureAppConfiguration.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HostBuilderExtensions
    {
        internal static readonly Dictionary<string, string> Environments = new Dictionary<string, string>
        {
            { "Development", "dev" },
            { "Staging", "qa" },
            { "Production", "prod" }
        };

        public static IHostBuilder UseAzureAppConfiguration(this IHostBuilder builder,
            Action<AppConfigurationConnectOptions, IConfiguration> configureConnect,
            Action<AppConfigurationWorkerOptions, IConfiguration> configureWorker)
        {
            builder.ConfigureHostConfiguration(configBuilder =>
            {
                configBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            });

            builder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                configBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                configBuilder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);

                configBuilder.AddAzureAppConfiguration(options =>
                {
                    var connectOptions = new AppConfigurationConnectOptions();
                    configureConnect?.Invoke(connectOptions, context.Configuration);

                    if (connectOptions.Endpoint != null)
                    {
                        var credentials = new DefaultAzureCredential();
                        options.Connect(connectOptions.Endpoint, credentials);
                    }

                    if (!string.IsNullOrEmpty(connectOptions.ConnectionString))
                    {
                        options.Connect(connectOptions.ConnectionString);
                    }

                    // Load configuration values with no label, which means all of the configurations that are not specific to
                    // Environment
                    //options.Select(KeyFilter.Any, LabelFilter.Null);

                    // Override with any configuration values specific to current hosting env
                    // options.Select(KeyFilter.Any, Environments[context.HostingEnvironment.EnvironmentName]);

                    options.ConfigureRefresh(refresh =>
                     {
                         refresh
                                .Register("Worker:Sentinel", refreshAll: true)
                               //.Register(KeyFilter.Any, LabelFilter.Null, true)
                                //  .Register(KeyFilter.Any, Environments[context.HostingEnvironment.EnvironmentName])
                                .SetCacheExpiration(TimeSpan.FromSeconds(10));
                     });
                });
            });

            builder.ConfigureServices((context, services) =>
            {
                services
                        .AddOptions<AppConfigurationWorkerOptions>()
                        .Configure(options =>
                        {
                            configureWorker?.Invoke(options, context.Configuration);
                        });

                services.AddHostedService<AppConfigurationWorker>();
            });

            return builder;
        }
    }
}
