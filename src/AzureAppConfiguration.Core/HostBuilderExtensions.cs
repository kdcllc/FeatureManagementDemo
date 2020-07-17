using System;
using System.Collections.Generic;

using Azure.Identity;

using AzureAppConfiguration.Core;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting
{
    public static class HostBuilderExtensions
    {
        private static readonly string AppConfig = nameof(AppConfig);

        private static readonly Dictionary<string, string> Environments = new Dictionary<string, string>
        {
            { "Development", "dev" },
            { "Staging", "qa" },
            { "Production", "prod" }
        };

        /// <summary>
        /// Adds Azure App Configuration Provider to <see cref="IHost"/> with ability to refresh the values.
        /// In addition the dev, qa and prod environment labels are registered with <see cref="IConfiguration"/> provider.
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// return Host.CreateDefaultBuilder(args)
        ///            .UseAzureAppConfiguration(
        /// "WorkerApp:WorkerOptions",
        ///            "WorkerApp:WorkerOptions:Message",
        ///            (connect, config) =>
        ///            {
        ///    config.Bind("AppConfig", connect);
        ///    },
        ///                    (interval, config) =>
        ///                    {
        ///                        interval.RefreshInterval = config.GetValue<TimeSpan>("AppConfig:RefreshInterval");
        ///                    })
        ///                    .ConfigureServices((hostContext, services) =>
        ///                    {
        ///    services.AddOptionsWithChangeToken<WorkerOptions>("WorkerApp:WorkerOptions", configureAction: (o) => { });
        ///    services.AddHostedService<Worker>();
        /// });
        /// ]]>
        /// </example>
        /// <param name="builder">The <see cref="IHostBuilder"/> builder.</param>
        /// <param name="appConfigiSectionName">
        /// The name of the configuration section that is to be registered to be retrieved from Azure App Configuration provider.
        /// </param>
        /// <param name="appConfigRefreshSectionName">
        /// The name of the configuration that is to be registered with Refresh values from Azure App Configuration provider.
        /// </param>
        /// <param name="configureAzureAppConfigOptions">
        /// The delegate that can overide the precious configurations.
        /// </param>
        /// <param name="configureConnect">
        /// The delegate action to configure <see cref="AppConfigurationConnectOptions"/>.
        /// This options provides with ability to configure method of authentication for the Azure.Idenity.
        /// If connection string is present it utilizes the full connection string; otherwise it will rely on Microsoft Managed Identity (MSI).
        /// </param>
        /// <param name="configureWorker">
        /// The delegate action to configure <see cref="AppConfigurationWorkerOptions"/>.
        /// This options provides with ability to trigger the refresh for registered options at specify interval.
        /// </param>
        /// <returns></returns>
        public static IHostBuilder UseAzureAppConfiguration(
            this IHostBuilder builder,
            string appConfigiSectionName,
            string appConfigRefreshSectionName,
            Action<AzureAppConfigurationOptions>? configureAzureAppConfigOptions = default,
            Action<AppConfigurationConnectOptions, IConfiguration>? configureConnect = default,
            Action<AppConfigurationWorkerOptions, IConfiguration>? configureWorker = default)
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
                    // create connection options
                    var connectOptions = new AppConfigurationConnectOptions();
                    context.Configuration.Bind(AppConfig, connectOptions);
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

                    options.ConfigureClientOptions(clientOptions => clientOptions.Retry.MaxRetries = 5);

                    // Load configuration values with no label, which means all of the configurations that are not specific to
                    // Environment
                    options.Select(appConfigiSectionName);

                    // Override with any configuration values specific to current hosting env
                    options.Select(appConfigiSectionName, Environments[context.HostingEnvironment.EnvironmentName]);

                    options.ConfigureRefresh(refresh =>
                    {
                        refresh
                            .Register(appConfigRefreshSectionName, refreshAll: true)
                            .Register(appConfigRefreshSectionName, Environments[context.HostingEnvironment.EnvironmentName], refreshAll: true)
                            .SetCacheExpiration(TimeSpan.FromSeconds(1));
                    });

                    options.UseFeatureFlags(flag => flag.Label = Environments[context.HostingEnvironment.EnvironmentName]);

                    configureAzureAppConfigOptions?.Invoke(options);
                });
            });

            builder.ConfigureServices((context, services) =>
            {
                services.AddOptionsWithChangeToken<AppConfigurationWorkerOptions>(
                    AppConfig,
                    configureOptions: options => configureWorker?.Invoke(options, context.Configuration));

                services.AddHostedService<AppConfigurationWorker>();
            });

            return builder;
        }
    }
}
