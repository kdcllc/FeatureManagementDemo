using Azure.Identity;
using AzureAppConfiguration.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Hosting
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
            string sectionName,
            string refreshSection,
            Action<AppConfigurationConnectOptions, IConfiguration> configureConnect,
            Action<AppConfigurationWorkerOptions, IConfiguration> configureWorker,
            Action<AzureAppConfigurationOptions>? configureOptions = default)
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

                    if (!string.IsNullOrEmpty(connectOptions.ConnectionString))
                    {
                        options.Connect(connectOptions.ConnectionString);
                    }
                    else if (connectOptions.Endpoint != null
                        && string.IsNullOrEmpty(connectOptions.ConnectionString))
                    {
                        var credentials = new DefaultAzureCredential();
                        options.Connect(connectOptions.Endpoint, credentials);
                    }

                    // Load configuration values with no label, which means all of the configurations that are not specific to
                    // Environment
                    options.Select(sectionName);

                    // Override with any configuration values specific to current hosting env
                    options.Select(sectionName, Environments[context.HostingEnvironment.EnvironmentName]);

                    options.ConfigureRefresh(refresh =>
                     {
                         refresh
                                //.Register("Worker:Sentinel", refreshAll: true)
                               .Register(refreshSection, refreshAll: true)
                               .Register(refreshSection, Environments[context.HostingEnvironment.EnvironmentName], refreshAll: true)
                               .SetCacheExpiration(TimeSpan.FromSeconds(1));
                     });

                    configureOptions?.Invoke(options);
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
