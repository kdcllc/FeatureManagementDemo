using Owin;

namespace FeatureManagementWeb.Legacy
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseAzureAppConfiguration();
        }
    }
}
