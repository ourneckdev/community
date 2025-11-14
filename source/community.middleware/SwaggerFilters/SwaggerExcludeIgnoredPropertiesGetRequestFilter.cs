using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace community.middleware.SwaggerFilters;

/// <summary>
///     Swagger filter that can be optionally applied to prevent output of properties whose serialization is hidden.
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class SwaggerExcludeIgnoredPropertiesGetRequestsFilter : IOperationFilter
{
    private readonly IList<JsonIgnoreCondition> _ignoredConditions =
        [JsonIgnoreCondition.Always, JsonIgnoreCondition.WhenWritingNull, JsonIgnoreCondition.WhenWritingDefault];

    /// <summary>
    ///     Applies the filter to Swagger
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription == null || operation.Parameters == null ||
            !context.ApiDescription.ParameterDescriptions.Any()) return;

        foreach (var parameter in context.ApiDescription.ParameterDescriptions
                     .Where(p => p.Source.Equals(BindingSource.Query) &&
                                 p.CustomAttributes().Any(a => a is JsonIgnoreAttribute attribute
                                                               && _ignoredConditions.Contains(attribute.Condition))))
            operation.Parameters.Remove(operation.Parameters.Single(w => w.Name.Equals(parameter.Name)));
    }
}