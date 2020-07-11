using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.FeatureManagement.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureManagement.Core.AspNetCore
{
    public class FeatureNotEnabledDisabledHandler : IDisabledFeaturesHandler
    {
        private readonly NotEnabledDisabledOptions options;

        public FeatureNotEnabledDisabledHandler(NotEnabledDisabledOptions options)
        {
            this.options = options;
        }

        public Task HandleDisabledFeatures(IEnumerable<string> features, ActionExecutingContext context)
        {
            var controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;

            if (options.ApiControllers.Contains(controllerName))
            {
                context.Result = new NotFoundResult();
            }
            else
            {
                var result = new ViewResult()
                {
                    ViewName = options.DefaultMvcViewPath,
                    ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                };

                result.ViewData["FeatureName"] = string.Join(", ", features);
                context.Result = result;
            }


            return Task.CompletedTask;
        }
    }
}
