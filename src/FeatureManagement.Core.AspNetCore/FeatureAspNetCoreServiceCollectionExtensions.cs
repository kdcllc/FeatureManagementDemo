using FeatureManagement.Core.AspNetCore.FeatureFilters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;

namespace FeatureManagement.Core.AspNetCore
{
    public static class FeatureAspNetCoreServiceCollectionExtensions
    {
        public static IFeatureManagementBuilder AddFeatures(this IServiceCollection services)
        {
            // Enable the use of IHttpContextAccessor
            services.AddHttpContextAccessor();

            return services.AddFeatureManagement()
                    .AddFeatureFilter<BrowserFilter>()
                    .AddFeatureFilter<PercentageFilter>()
                    .UseDisabledFeaturesHandler(new FeatureNotEnabledDisabledHandler());
        }
    }
}
