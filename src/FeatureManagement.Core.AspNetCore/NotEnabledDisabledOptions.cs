using System.Collections.Generic;

namespace FeatureManagement.Core.AspNetCore
{
    public class NotEnabledDisabledOptions
    {
        public string DefaultMvcViewPath { get; set; } = "Views/Shared/FeatureNotEnabled.cshtml";

        public List<string> ApiControllers { get; set; } = new List<string>();
    }
}
