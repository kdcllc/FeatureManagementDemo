using System;

namespace AzureAppConfiguration.Core
{
    public class AppConfigurationConnectOptions
    {
        public string? ConnectionString { get; set; }

        public Uri? Endpoint { get; set; }
    }
}
