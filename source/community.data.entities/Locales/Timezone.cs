using community.common.BaseClasses;

namespace community.data.entities.Locales;

/// <summary>
///     Represents a postgres identified timezone
/// </summary>
public class TimeZone : BaseLocaleEntity
{
    /// <summary>
    ///     gets or sets the utc offset of the time zone
    /// </summary>
    public TimeSpan UtcOffset { get; set; }
}