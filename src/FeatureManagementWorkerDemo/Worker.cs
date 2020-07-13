using System;
using System.Threading;
using System.Threading.Tasks;

using FeatureManagement.Core;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;

namespace FeatureManagementWorkerDemo
{
    public class Worker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Worker> _logger;
        private WorkerOptions _options;

        public Worker(
            IOptionsMonitor<WorkerOptions> optionsMonitor,
            IServiceProvider serviceProvider,
            ILogger<Worker> logger)
        {
            _options = optionsMonitor.CurrentValue;
            optionsMonitor.OnChange(n => _options = n);

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time} with Message: {message}", DateTimeOffset.Now, _options.Message);

                using var scope = _serviceProvider.CreateScope();
                var featureManager = scope.ServiceProvider.GetRequiredService<IFeatureManagerSnapshot>();

                _logger.LogInformation("Cached Flag is:{cachedFlag}", await featureManager.IsEnabledAsync(nameof(FeatureFlags.Cached)));

                _logger.LogInformation("Alpha Flag is: {alphaFlag}", await featureManager.IsEnabledAsync(nameof(FeatureFlags.Alpha)));

                _logger.LogInformation("Beta Flag is: {betaFlag}", await featureManager.IsEnabledAsync(nameof(FeatureFlags.Beta)));

                await Task.Delay(_options.Interval, stoppingToken);
            }
        }
    }
}
