using community.data.entities.Locales;

namespace community.models.Responses.Locales;

/// <summary>
///     Defines a county response
/// </summary>
public record CountyResponse(
    string Code,
    string StateCode,
    string CountryCode,
    string Name)
{
    /// <summary>
    ///     Maps a county database entity to a response object.
    /// </summary>
    /// <param name="county">The database entity</param>
    /// <returns>a hydrated response object.</returns>
    public static implicit operator CountyResponse(County county)
    {
        return new CountyResponse(county.Code, 
            county.StateCode,
            county.CountryCode,
            county.Name);
    }
}