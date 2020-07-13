using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.FeatureManagement.Mvc;

namespace FeatureManagement.Core.AspNetCore
{
    public class FeatureNotEnabledDisabledHandler : IDisabledFeaturesHandler
    {
        private readonly NotEnabledDisabledOptions _options;

        public FeatureNotEnabledDisabledHandler(NotEnabledDisabledOptions options)
        {
            _options = options;
        }

        public Task HandleDisabledFeatures(IEnumerable<string> features, ActionExecutingContext context)
        {
            var controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;

            if (_options.ApiControllers.Contains(controllerName))
            {
                context.Result = new NotFoundResult();
            }
            else
            {
                var result = new ViewResult()
                {
                    ViewName = _options.DefaultMvcViewPath,
                    ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                };

                result.ViewData["FeatureName"] = string.Join(", ", features);
                context.Result = result;
            }

            return Task.CompletedTask;
        }
    }
}
