# AzureAppConfiguration.Core

App Configuration offers the following benefits:

- A fully managed service that can be set up in minutes
- Flexible key representations and mappings
- Tagging with labels
- Point-in-time replay of settings
- Dedicated UI for feature flag management
- Comparison of two sets of configurations on custom-defined dimensions
- Enhanced security through Azure-managed identities
- Encryption of sensitive information at rest and in transit
- Native integration with popular frameworks

- Centralize management and distribution of hierarchical configuration data for different environments and geographies
- Dynamically change application settings without the need to redeploy or restart an application
- Control feature availability in real-time

- Labels: Tag your key-values with labels to enhance the experience, and to use the same keys for multiple applications with different values.
- Configuration comparison: Enables you to easily compare various values of the same key, or other things you'll need to verify easily.
- Built-in support for Azure Managed Identity, so our web apps/functions/applications can easily access the key-values without specifying credentials in code or config.
- Centralized management of all configurations across all our environments we're operating
- Content Types


Keys stored in App Configuration are case-sensitive, unicode-based strings. The keys app1 and App1 are distinct in an App Configuration store.
You can use any unicode character in key names except for *, ,, and \. If you need to include one of these reserved characters, escape it by using \{Reserved Character}.