using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace community.middleware.SwaggerFilters;

/// <summary>
///     Sets up an HTTP header for correlation key
/// </summary>
public class CorrelationIdOperationFilter : IOperationFilter
{
    /// <summary>
    ///     Applies the Operation filter to the Swagger Gen.
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "Correlation-Key",
            In = ParameterLocation.Header,
            Description = "Correlation key used to debug API operations by tracking a request through to completion.",
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = "string",
                Format = "uuid",
                Default = new OpenApiString($"{Guid.NewGuid()}")
            }
        });
    }
}