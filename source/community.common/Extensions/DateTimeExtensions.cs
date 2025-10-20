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
    public static string ToUnixTime(this DateTime dateTime)
    {
        dateTime = dateTime.ToUniversalTime();
        return $"{((DateTimeOffset)dateTime).ToUnixTimeSeconds()}";
    }
}