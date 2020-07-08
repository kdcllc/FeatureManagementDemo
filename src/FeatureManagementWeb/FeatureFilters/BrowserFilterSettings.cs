using System.Collections.Generic;

namespace FeatureManagementWeb.FeatureFilters
{
    public class BrowserFilterSettings
    {
        public IList<string> AllowedBrowsers { get; set; } = new List<string>();
    }
}
