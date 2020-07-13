using System;

using FeatureManagement.Core.AspNetCore;
using FeatureManagement.Core.AspNetCore.FeatureFilters;

using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.FeatureManagement
{
    public static class AspNetCoreFeatureManagementBuilderExtensions
    {
        /// <summary>
        /// Adds functionality for MVC and also API Controllers: ViewData["FeatureName"].
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IFeatureManagementBuilder AddAspNetCoreFeatures(
            this IFeatureManagementBuilder builder,
            Action<NotEnabledDisabledOptions> configure)
        {
            var options = new NotEnabledDisabledOptions();
            configure(options);

            // Enable the use of IHttpContextAccessor
            builder.Services.AddHttpContextAccessor();

            return builder
                    .AddFeatureFilter<BrowserFilter>()
                    .UseDisabledFeaturesHandler(new FeatureNotEnabledDisabledHandler(options));
        }
    }
}
