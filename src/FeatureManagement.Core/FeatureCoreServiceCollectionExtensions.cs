using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FeatureCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddOptionsWithChangeToken<TOptions>(
            this IServiceCollection services,
            string sectionName,
            string? optionName = default,
            Action<TOptions>? configureAction = default) where TOptions : class, new()
        {
            // configure changeable configurations
            services.RegisterInternal<TOptions>(sectionName, optionName);

            // create options instance from the configuration
            services.AddTransient((Func<IServiceProvider, IConfigureNamedOptions<TOptions>>)(sp =>
            {
                return new ConfigureNamedOptions<TOptions>(optionName, options =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    configuration.Bind(sectionName, options);

                    configureAction?.Invoke(options);
                });
            }));

            // Registers an IConfigureOptions<TOptions> action configurator. Being last it will bind from config source first
            // and run the customization afterwards
            services
                .AddOptions<TOptions>(optionName)
                .Configure(options => configureAction?.Invoke(options));

            return services;
        }

        private static void RegisterInternal<TOptions>(
            this IServiceCollection services,
            string sectionName,
            string? optionName = null)
            where TOptions : class, new()
        {
            if (optionName == null)
            {
                optionName = Options.Options.DefaultName;
            }

            services.AddSingleton((Func<IServiceProvider, IOptionsChangeTokenSource<TOptions>>)((sp) =>
            {
                var config = sp.GetRequiredService<IConfiguration>().GetSection(sectionName);
                return new ConfigurationChangeTokenSource<TOptions>(optionName, config);
            }));

            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<TOptions>>().Value);

            services.AddSingleton((Func<IServiceProvider, IConfigureOptions<TOptions>>)(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>().GetSection(sectionName);
                return new NamedConfigureFromConfigurationOptions<TOptions>(optionName, config);
            }));
        }
    }
}
