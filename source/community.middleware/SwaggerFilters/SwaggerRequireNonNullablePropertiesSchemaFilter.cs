using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace community.middleware.SwaggerFilters;

/// <summary>
///     Indicates required fields for non-nullable properties on objects defined in the schema
/// </summary>
public class SwaggerRequireNonNullablePropertiesSchemaFilter : ISchemaFilter
{
    /// <summary>
    ///     Applies Required = true to the schema's model properties.
    /// </summary>
    /// <param name="model">The model the operation filter is being applied to.</param>
    /// <param name="context">The Schema Filter context.</param>
    /// <exception cref="NotImplementedException"></exception>
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        FixNullableProperties(model, context);

        var requiredProps = model.Properties
            .Where(s => !s.Value.Nullable && !model.Required.Contains(s.Key))
            .Select(s => s.Key);

        foreach (var propertyKey in requiredProps) model.Required.Add(propertyKey);
    }

    private static void FixNullableProperties(OpenApiSchema schema, SchemaFilterContext context)
    {
        foreach (var property in schema.Properties)
            if (property.Value.Reference != null)
            {
                var field = context.Type
                    .GetMembers(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(x =>
                        string.Equals(x.Name, property.Key, StringComparison.InvariantCultureIgnoreCase));

                if (field != null)
                {
                    var fieldType = field switch
                    {
                        FieldInfo fieldInfo => fieldInfo.FieldType,
                        PropertyInfo propertyInfo => propertyInfo.PropertyType,
                        _ => throw new NotSupportedException()
                    };

                    property.Value.Nullable = fieldType.IsValueType
                        ? Nullable.GetUnderlyingType(fieldType) != null
                        : !field.IsNonNullableReferenceType();
                }
            }
    }
}