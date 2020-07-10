﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AzureAppConfiguration.Core
{
    public class AppConfigurationWorker : BackgroundService
    {
        private AppConfigurationWorkerOptions _options;
        private readonly ILogger<AppConfigurationWorker> _logger;

        public List<IConfigurationRefresher> Refreshers { get; }

        public AppConfigurationWorker(
            IOptionsMonitor<AppConfigurationWorkerOptions> optionsMonitor,
            IConfiguration configuration,
            ILogger<AppConfigurationWorker> logger)
        {
            _options = optionsMonitor.CurrentValue;

            optionsMonitor.OnChange(n => _options = n);

            Refreshers = new List<IConfigurationRefresher>();
            var configurationRoot = configuration as IConfigurationRoot;

            if (configurationRoot == null)
            {
                throw new InvalidOperationException("Unable to access the Azure App Configuration provider. Please ensure that it has been configured correctly.");
            }

            foreach (var provider in configurationRoot.Providers)
            {
                if (provider is IConfigurationRefresher refresher)
                {
                    Refreshers.Add(refresher);
                }
            }

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("{name} running Refresh: {time}", nameof(AppConfigurationWorker), DateTimeOffset.Now);

                foreach (var refresher in Refreshers)
                {
                    var result = await refresher.TryRefreshAsync();
                }

                await Task.Delay(_options.RefreshInterval, stoppingToken);
            }
        }
    }
}
