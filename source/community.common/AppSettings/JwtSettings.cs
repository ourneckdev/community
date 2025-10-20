namespace community.common.AppSettings;

/// <summary>
///     Represents the app setting JwtSettings section.
/// </summary>
public class JwtSettings
{
    /// <summary>
    ///     Defines the issuer of the JWT.
    /// </summary>
    public string Issuer { get; set; } = "";

    /// <summary>
    ///     Defines the intended audience of the JWT.
    /// </summary>
    public string Audience { get; set; } = "";

    /// <summary>
    ///     Gets or sets the symmetric key used to sign the JWT.
    /// </summary>
    public string Secret { get; set; } = "";

    /// <summary>
    ///     Represents a time span for how long the JWT is valid.
    /// </summary>
    public TimeSpan TokenLifetime { get; set; }

    /// <summary>
    ///     Represents a time span for how long the refresh token is valid.
    /// </summary>
    public TimeSpan RefreshTokenLifetime { get; set; }
}