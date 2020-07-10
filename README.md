# Feature Management Workshop

DotNetCore Feature Management Demo for AspNetCore and Worker templates

**Don't try to protect every code path in the new feature code with a toggle, focus on just the entry points that would lead users there and toggle those entry points. If you find that creating, maintaining, or removing the toggles takes significant time, then that's a sign that you have too many toggle tests. Martin Fowler**

## Feature Management Basics

- [Azure App Configuration Keys and Values Concepts](https://docs.microsoft.com/en-us/azure/azure-app-configuration/concept-key-value)
- [Feature Management Concepts](https://docs.microsoft.com/en-us/azure/azure-app-configuration/concept-feature-management)

## Azure Specific

- [Create and Manage Feature Flags with Azure App Configuration](https://docs.microsoft.com/en-us/azure/azure-app-configuration/manage-feature-flags)



## Demo Applications

- [FeatureManagementWeb](./src/FeatureManagementWeb) - provides with basic demonstrations of the features.

### Release Features/Toggles

- **Code branch management:** Use feature flags to wrap new application functionality currently under development. Such functionality is "hidden" by default. You can safely ship the feature, even though it's unfinished, and it will stay dormant in production. Using this approach, called dark deployment, you can release all your code at the end of each development cycle. You no longer need to maintain code branches across multiple development cycles because a given feature requires more than one cycle to complete.

- **Instant kill switch:** Feature flags provide an inherent safety net for releasing new functionality. You can turn application features on and off without redeploying any code. If necessary, you can quickly disable a feature without rebuilding and redeploying your application.

### Experiment Features/Toggles

- **Test in production:** Use feature flags to grant early access to new functionality in production. For example, you can limit access to team members or to internal beta testers. These users will experience the full-fidelity production experience instead of a simulated or partial experience in a test environment.

- **Flighting:** Use feature flags to incrementally roll out new functionality to end users. You can target a small percentage of your user population first and increase that percentage gradually over time.

### Permissioning Features/Toggles or Business Feature/Toggles

**Selective activation:** Use feature flags to segment your users and deliver a specific set of features to each group. You may have a feature that works only on a certain web browser. You can define a feature flag so that only users of that browser can see and use the feature. With this approach, you can easily expand the supported browser list later without having to make any code changes.


## References

- [Martin Fowler FeatureToggle Pattern](https://martinfowler.com/bliki/FeatureToggle.html)

## Azure and Microsoft Repos

- [FeatureManagement-Dotnet](https://github.com/microsoft/FeatureManagement-Dotnet)
- [Azure App Configuration - .NET Standard](https://github.com/Azure/AppConfiguration-DotnetProvider)
- [Azure SDK for .NET](https://github.com/Azure/azure-sdk-for-net)

## Andrew Lock's series

- [Introducing Microsoft.FeatureManagement](https://andrewlock.net/introducing-the-microsoft-featuremanagement-library-adding-feature-flags-to-an-asp-net-core-app-part-1/)
- [Filtering action methods with feature flags](https://andrewlock.net/filtering-action-methods-with-feature-flags-adding-feature-flags-to-an-asp-net-core-app-part-2/)
- [Creating dynamic feature flags with feature filters](https://andrewlock.net/creating-dynamic-feature-flags-with-feature-filters-adding-feature-flags-to-an-asp-net-core-app-part-3/)
- [Creating a custom feature filter](https://andrewlock.net/creating-a-custom-feature-filter-adding-feature-flags-to-an-asp-net-core-app-part-4/)
- [Ensuring consistent feature flags across requests](https://andrewlock.net/keeping-consistent-feature-flags-across-requests-adding-feature-flags-to-an-asp-net-core-app-part-5/)
- [Alternatives to Microsoft.FeatureManagement](https://andrewlock.net/alternatives-to-microsoft-featuremanagement/)