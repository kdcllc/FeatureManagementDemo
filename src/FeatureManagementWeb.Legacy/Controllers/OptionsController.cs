using System.Collections.Generic;
using System.Web.Http;

using Bet.AspNet.FeatureManagement;

using FeatureManagementWeb.Legacy.Options;

using Microsoft.Extensions.Options;

namespace FeatureManagementWeb.Legacy.Controllers
{
    public class OptionsController : ApiController
    {
        private readonly IOptionsSnapshot<AppOptions> _options;

        public OptionsController(IOptionsSnapshot<AppOptions> options)
        {
            _options = options;
        }

        // GET: api/Options
        [ApiFeatureGate(FeatureReleaseFlags.Alpha)]
        public IEnumerable<string> Get()
        {
            return new string[] { _options.Value.Message };
        }
    }
}
