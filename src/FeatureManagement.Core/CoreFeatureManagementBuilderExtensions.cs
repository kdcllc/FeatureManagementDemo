using Microsoft.FeatureManagement.FeatureFilters;

namespace Microsoft.FeatureManagement
{
    public static class CoreFeatureManagementBuilderExtensions
    {
        public static IFeatureManagementBuilder AddDefaultFeatureManagement(this IFeatureManagementBuilder builder)
        {
            builder.AddFeatureFilter<PercentageFilter>();
            builder.AddFeatureFilter<TimeWindowFilter>();

            return builder;
        }
    }
}
