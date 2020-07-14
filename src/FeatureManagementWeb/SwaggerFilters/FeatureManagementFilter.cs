using System;
using System.Linq;

using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Swashbuckle.AspNetCore.Filters
{
    public class FeatureManagementFilter : SwaggerGen.IDocumentFilter
    {
        private readonly IServiceProvider _provider;

        public FeatureManagementFilter(IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var contextApiDescription in context.ApiDescriptions)
            {
                var actionDescriptor = (ControllerActionDescriptor)contextApiDescription.ActionDescriptor;

                if (actionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(FeatureGateAttribute), true).Length > 0 ||
                    actionDescriptor.MethodInfo.GetCustomAttributes(typeof(FeatureGateAttribute), true).Length > 0)
                {
                    using var scope = _provider.CreateScope();
                    var featureManager = scope.ServiceProvider.GetRequiredService<IFeatureManagerSnapshot>();

                    var actionAttributes = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(FeatureGateAttribute), true);

                    foreach (var attr in actionAttributes)
                    {
                        foreach (var feature in (attr as FeatureGateAttribute).Features)
                        {
                            if (!featureManager.IsEnabledAsync(feature).GetAwaiter().GetResult())
                            {
                                var key = "/" + contextApiDescription.RelativePath.TrimEnd('/');

                                var operation = (OperationType)Enum.Parse(typeof(OperationType), contextApiDescription.HttpMethod, true);

                                swaggerDoc.Paths[key].Operations.Remove(operation);

                                // drop the entire route of there are no operations left
                                if (!swaggerDoc.Paths[key].Operations.Any())
                                {
                                    swaggerDoc.Paths.Remove(key);
                                }

                                swaggerDoc.Components?.Schemas?.Clear();

                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
