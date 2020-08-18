using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Bet.AspNet.FeatureManagement;
using Bet.AspNet.LegacyHosting;

using FeatureManagementWeb.Legacy.Options;
using FeatureManagementWeb.Legacy.Service;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace FeatureManagementWeb.Legacy
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = WebHost.CreateDefaultBuilder<Startup>()

                                 // requires to have configuration for Azure App Configurations
                                .UseAzureAppConfiguration("WebApp:AppOptions*", "WebApp:AppOptions:Flag")

                                .ConfigureServices((context, services) =>
                                {
                                    services.AddChangeTokenOptions<AppOptions>("AppOptions", configureAction: (_) => { });
                                    services.AddChangeTokenOptions<AppOptions>("WebApp:AppOptions", configureAction: (_) => { });

                                    services.AddFeatureManagement();

                                    services.Configure<FeatureGateOptions>(options =>
                                    {
                                        options.ControllerName = "Home";
                                        options.ActionName = "Index";
                                    });

                                    services.AddSingleton<ConfigurationService>();
                                })
                                .Build();

            // Configure DI for Mvc4 and WebApi2 Controllers
            builder.ConfigureMvcDependencyResolver();

            // Configure DI for WebForms
            builder.ConfigureWebFormsResolver();

            var logger = builder.Services.GetService<ILoggerFactory>().CreateLogger(nameof(MvcApplication));

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
