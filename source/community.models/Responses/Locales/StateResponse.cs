using community.data.entities.Locales;

namespace community.models.Responses.Locales;

/// <summary>
///     Defines a state response object.
/// </summary>
/// <param name="Code"></param>
/// <param name="CountryCode"></param>
/// <param name="FipsCode"></param>
/// <param name="AnsiCode"></param>
/// <param name="Name"></param>
public record StateResponse(
    string Code,
    string CountryCode,
    string? FipsCode,
    string? AnsiCode,
    string Name)
{
    /// <summary>
    ///     Maps a database state entity to a response object.
    /// </summary>
    /// <param name="state">The database entity to map</param>
    /// <returns>A hydrated response object.</returns>
    public static implicit operator StateResponse(State state)
    {
        return new StateResponse(state.Code, state.CountryCode, state.FipsCode, state.AnsiCode, state.Name);
    }
}