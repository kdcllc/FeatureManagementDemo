using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FeatureManagementWorkerDemo
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private WorkerOptions _options;

        public Worker(IOptionsMonitor<WorkerOptions> optionsMonitor, ILogger<Worker> logger)
        {
            _options = optionsMonitor.CurrentValue;
            optionsMonitor.OnChange(n => _options = n);

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time} with Message: {message}", DateTimeOffset.Now, _options.Message);
                await Task.Delay(_options.Interval, stoppingToken);
            }
        }
    }
}
