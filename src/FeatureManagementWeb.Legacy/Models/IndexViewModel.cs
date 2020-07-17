using FeatureManagementWeb.Legacy.Options;

using Microsoft.FeatureManagement;

namespace FeatureManagementWeb.Legacy.Models
{
    public class IndexViewModel : BaseViewModel
    {
        public IndexViewModel(IFeatureManagerSnapshot featureManager) : base(featureManager)
        {
        }

        public AppOptions Options { get; set; }
    }
}
