namespace community.common.Extensions;

/// <summary>
///     Defines extensions to the DateTime struct.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    ///     Generates an epoch time.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long ToUnixTime(this DateTime dateTime)
    {
        dateTime = dateTime.ToUniversalTime();
        return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
    }

    /// <summary>
    /// Converts epoch time to a UTC Date.
    /// </summary>
    /// <param name="epoch"></param>
    /// <returns></returns>
    public static DateTime FromUnixTime(this long epoch)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return dateTime.AddSeconds(epoch);
    }

}