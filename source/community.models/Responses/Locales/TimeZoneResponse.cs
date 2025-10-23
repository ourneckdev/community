using TimeZone = community.data.entities.Locales.TimeZone;

namespace community.models.Responses.Locales;

/// <summary>
///     Immutable response object that represents available timezones.
/// </summary>
/// <param name="Code">The abbreviation of the timezone</param>
/// <param name="Name">the name of the timezone</param>
/// <param name="UtcOffset">The offset from UTC of the timezone</param>
public record TimeZoneResponse(
    string Code,
    string Name,
    TimeSpan UtcOffset)
{
    /// <summary>
    ///     Maps a timezone entity to it's response object.
    /// </summary>
    /// <param name="timezone"></param>
    /// <returns></returns>
    public static implicit operator TimeZoneResponse(TimeZone timezone)
    {
        return new TimeZoneResponse(timezone.Code, timezone.Name, timezone.UtcOffset);
    }
}