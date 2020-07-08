using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
