using System;

namespace AzureAppConfiguration.Core
{
    public class AppConfigurationWorkerOptions
    {
        public TimeSpan? RefreshInterval { get; set; } = TimeSpan.FromSeconds(30);
    }
}
