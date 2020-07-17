using System.Web;
using System.Web.Mvc;

using Bet.AspNet.FeatureManagement;

using FeatureManagementWeb.Legacy.Models;
using FeatureManagementWeb.Legacy.Options;
using FeatureManagementWeb.Legacy.Service;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;

namespace FeatureManagementWeb.Legacy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ConfigurationService _optionsService;
        private readonly IFeatureManagerSnapshot _featureManager;
        private AppOptions _options;

        public HomeController(
            IOptionsMonitor<AppOptions> optionsMonitor,
            ConfigurationService optionsService)
        {
            _options = optionsMonitor.CurrentValue;
            optionsMonitor.OnChange(n => _options = n);

            _optionsService = optionsService;
            _featureManager = Get();
        }

        public ActionResult Index()
        {
            ViewData["Message"] = _optionsService.Referesh();

            var model = new IndexViewModel(_featureManager)
            {
                Options = _options,
            };

            return View(model);
        }

        [FeatureGate(RequirementType.All, FeatureReleaseFlags.Beta, FeatureReleaseFlags.Alpha)]
        public ActionResult Beta()
        {
            ViewBag.Message = "Your application description page.";

            var model = new BaseViewModel(_featureManager);

            return View(model);
        }

        private IFeatureManagerSnapshot Get()
        {
            using (var scope = HttpRuntime.WebObjectActivator.CreateScope())
            {
                if (scope != null)
                {
                    return scope.ServiceProvider.GetRequiredService<IFeatureManagerSnapshot>();
                }
            }
            return null;
        }
    }
}
