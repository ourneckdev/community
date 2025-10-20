using System.Diagnostics.CodeAnalysis;

namespace community.middleware.Configurations;

/// <summary>
///     Model used for initializing the Swagger documentation produced by the API.
/// </summary>
[ExcludeFromCodeCoverage]
public class SwaggerConfiguration
{
    /// <summary>
    ///     Gets or sets the version of the API
    /// </summary>
    public string Version { get; set; } = "v1";

    /// <summary>
    ///     Gets or sets the title of the API being documented.
    /// </summary>
    public string Title { get; set; } = nameof(Title);

    /// <summary>
    ///     Gets a description of the API to display.
    /// </summary>
    public string Description { get; set; } = nameof(Description);

    /// <summary>
    ///     Gets or sets the documentation files to include for outputting models to the SwaggerGen page
    /// </summary>
    public IEnumerable<string> DocumentationFiles { get; set; } = new List<string>();

    /// <summary>
    /// Whether or not the authorization button displays in swagger
    /// </summary>
    public bool ShouldDisplayAuthorization { get; set; } = true;
}