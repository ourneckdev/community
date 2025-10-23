using community.data.entities.Locales;

namespace community.models.Responses.Locales;

/// <summary>
///     Gets an immutable country
/// </summary>
/// <param name="Code"></param>
/// <param name="Name"></param>
/// <param name="Iso2"></param>
/// <param name="NumericCode"></param>
public record CountryResponse(
    string Code,
    string Name,
    string Iso2,
    string NumericCode)
{
    /// <summary>
    ///     Maps a <see cref="Country" /> entity to an immutable response object
    /// </summary>
    /// <param name="country"></param>
    /// <returns></returns>
    public static implicit operator CountryResponse(Country country)
    {
        return new CountryResponse(country.Code, country.Name, country.Iso2Code, country.NumericCode);
    }
}