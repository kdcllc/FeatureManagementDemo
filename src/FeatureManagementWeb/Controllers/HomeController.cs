using FeatureManagement.Core;
using FeatureManagementWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FeatureManagementWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFeatureManagerSnapshot _featureSnapshot;

        public HomeController(ILogger<HomeController> logger, IFeatureManagerSnapshot featureSnapshot)
        {
            _logger = logger;
            _featureSnapshot = featureSnapshot;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Message"] = "Welcome";

            if (await _featureSnapshot.IsEnabledAsync(nameof(FeatureFlags.BrowserRenderer)))
            {
                ViewData["Message"] = $"Welcome! You can see this message only if '{nameof(FeatureFlags.BrowserRenderer)}' is enabled.";
            };

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
