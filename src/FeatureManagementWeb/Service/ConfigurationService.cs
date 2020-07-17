using System;
using System.Threading;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace FeatureManagementWeb.Service
{
    public class ConfigurationService : IDisposable
    {
        private readonly IConfiguration _configuration;

        private IDisposable _changeSubscription;
        private int _stale = 0;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _changeSubscription = ChangeToken.OnChange(
                () => _configuration.GetReloadToken(),
                () => _stale = 1);
        }

        /// <summary>
        /// Return true if <see cref="IConfiguration"/> changed.
        /// </summary>
        /// <returns></returns>
        public bool Referesh()
        {
            return Interlocked.Exchange(ref _stale, 0) != 0;
        }

        public void Dispose()
        {
            _changeSubscription?.Dispose();
            _changeSubscription = null;
        }
    }
}
