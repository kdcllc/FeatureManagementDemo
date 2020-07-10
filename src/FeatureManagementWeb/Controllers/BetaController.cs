using FeatureManagement.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace FeatureManagementWeb.Controllers
{
    public class BetaController : Controller
    {
        [FeatureGate(RequirementType.All, FeatureFlags.Beta, FeatureFlags.Alpha)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
