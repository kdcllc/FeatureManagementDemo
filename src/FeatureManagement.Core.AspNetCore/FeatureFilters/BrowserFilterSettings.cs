using System.Collections.Generic;

namespace FeatureManagement.Core.AspNetCore.FeatureFilters
{
    public class BrowserFilterSettings
    {
        public IList<string> AllowedBrowsers { get; set; } = new List<string>();
    }
}
